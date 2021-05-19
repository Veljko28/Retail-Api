using Microsoft.IdentityModel.Tokens;
using Retail_Api.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models.Requests;
using Retail_Api.Services;
using Retail_Api.Repositories.Services;

namespace Retail_Api.Models.Services
{
	public class IdentityService : IIdentityService
	{

		private readonly JwtSettings _jwtSettings;
		private readonly IConfiguration _configuration;
		private readonly IRefreshTokenDb _tokenDb;

		public IdentityService(JwtSettings jwtSettings, IConfiguration configuration, IRefreshTokenDb tokenDb)
		{
			_jwtSettings = jwtSettings;
			_configuration = configuration;
			_tokenDb = tokenDb;
		}


		private async Task<User> getUserByIdAsync(string id)
		{
			string sql = "exec [GetUserById] @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var response = await db.QueryAsync<User>(sql, new { Id = id });

				return response.FirstOrDefault();
			}
		}


		

		public void addRoleToList(string roleName,string roleClaim, List<string> roleNames, List<Claim> roleClaims)
		{
			if (roleNames.Contains(roleName))
			{
				roleClaims.Add(new Claim(roleClaim, "true"));
			}
		}
	

		public async Task<UserRequest> RegisterAsync(UserRequest userRequest)
		{

			string sql = "exec [AddUser] @Id, @FirstName, @LastName, @Password, @EmailAddress, @DateCreated";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, new {
					Id = Guid.NewGuid().ToString(),
					EmailAddress = userRequest.EmailAddress,
					DateCreated = DateTime.UtcNow,
					FirstName = userRequest.FirstName,
					LastName = userRequest.LastName,
					Password = userRequest.Password,
				}); ;

				if (rowsModified > 0)
				{
					return userRequest;
				}

				return null;
			}


		}


		public async Task<TokenResponse> LoginAsync(string email, string password)
		{
			User loggedInUser = new User();

			string sql = "exec [GetUser] @EmailAddress, @Password";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				var result = await db.QueryAsync<User>(sql, new
				{
					EmailAddress = email,
					Password = password
				});
				loggedInUser = result.FirstOrDefault();
			}

			if (loggedInUser == null)
			{
				return null;
			}

			var roles = await RoleRepository.getUserRolesAsync(loggedInUser.Id, _configuration);

			List<string> roleNames = new List<string>();

			foreach (Role r in roles)
			{
				roleNames.Add(await RoleRepository.getRoleNameAsync(r.RoleId, _configuration));
			}

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			
			byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

			List<Claim> roleClaims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, email),
				new Claim("id", loggedInUser.Id),
			};
		
			foreach (string role in roleNames)
			{
				if (role == "Admin")
				{
					addRoleToList(role, "adn", roleNames, roleClaims);
				}

				if (role == "Manager")
				{
					addRoleToList(role, "mng", roleNames, roleClaims);
				}

				if (role == "Cashier")
				{
					addRoleToList(role, "csr", roleNames, roleClaims);
				}
			}
		

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(roleClaims),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			var refreshToken = new RefreshToken
			{
				Token = tokenHandler.WriteToken(token),
				JwtId = token.Id,
				UserId = loggedInUser.Id,
				CreatedDate = DateTime.UtcNow,
				Expires = DateTime.UtcNow.AddMonths(6),

			};

			bool addedToken = await _tokenDb.addTokenAsync(refreshToken);

			if (!addedToken)
			{
				return null;
			}

			return new TokenResponse
			{
				Token = tokenHandler.WriteToken(token),
				RefreshToken = refreshToken.JwtId,
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime).ToString()
			};
		}

		public async Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken)
		{
			var validatedToken = CheckRole.getPrincipalFromToken(token, _configuration);

			if (validatedToken == null)
			{
				return null;
			}

			var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

			var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

			if (expiryDateTimeUtc > DateTime.UtcNow)
			{
				return null;
			}

			var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;


			var storedRefreshToken = await _tokenDb.findTokenAsync(token);

			if (storedRefreshToken == null || DateTime.UtcNow > storedRefreshToken.Expires || storedRefreshToken.Invalidated ||
				storedRefreshToken.Used || storedRefreshToken.JwtId != jti)
			{
				return null;
			}


			storedRefreshToken.Used = true;
			bool result = await _tokenDb.updateTokenAsync(storedRefreshToken);

			if (!result)
			{
				return null;
			}

			User currentUser = await getUserByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

			if (currentUser == null)
			{
				return null;
			}
			else return await LoginAsync(currentUser.EmailAddress, currentUser.Password);
		}
	}
}
