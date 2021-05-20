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
using System.Security.Cryptography;

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

			// HASHING PASSWORD BEGIN 

			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			var pbkdf2 = new Rfc2898DeriveBytes(userRequest.Password, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);


			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			string passwordHash = Convert.ToBase64String(hashBytes);

			// HASHING PASSWORD END

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, new {
					Id = Guid.NewGuid().ToString(),
					EmailAddress = userRequest.EmailAddress,
					DateCreated = DateTime.UtcNow,
					FirstName = userRequest.FirstName,
					LastName = userRequest.LastName,
					Password = passwordHash,
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
			LoginResponse loginResponse = new LoginResponse();
			string sql = "exec [GetPasswordByEmail] @EmailAddress";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				var result = await db.QueryAsync<LoginResponse>(sql, new
				{
					EmailAddress = email
				});
				loginResponse = result.FirstOrDefault();
			}

			if (loginResponse == null)
			{
				return null;
			}

			// DEHASH THE PASSWORD STORED IN THE DATABASE
			byte[] hashBytes = Convert.FromBase64String(loginResponse.Password);
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);

			var passHash = new Rfc2898DeriveBytes(password, salt, 100000);
			byte[] hash = passHash.GetBytes(20);

			// COMPARE THE RESULTS 

			for (int i = 0; i < 20; i++)
				if (hashBytes[i + 16] != hash[i])
					return null;



			var roles = await RoleRepository.getUserRolesAsync(loginResponse.Id, _configuration);

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
				new Claim("id", loginResponse.Id),
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
				UserId = loginResponse.Id,
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
