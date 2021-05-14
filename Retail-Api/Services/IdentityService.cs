using Microsoft.AspNetCore.Identity;
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

namespace Retail_Api.Models.Services
{
	public class IdentityService : IIdentityService
	{
		//private readonly UserManager<IdentityUser> _userManager;

		//public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
		//{
		//	_userManager = userManager;
		//	_jwtSettings = jwtSettings;
		//}

		private readonly JwtSettings _jwtSettings;
		private readonly IConfiguration _configuration;
		private readonly IRefreshTokenDb _tokenDb;

		public IdentityService(JwtSettings jwtSettings, IConfiguration configuration, IRefreshTokenDb tokenDb)
		{
			_jwtSettings = jwtSettings;
			_configuration = configuration;
			_tokenDb = tokenDb;
		}


		private async Task<User> getUserByIdAsync(int id)
		{
			string sql = "exec [GetUserById] @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var response = await db.QueryAsync<User>(sql, new { Id = id });

				return response.FirstOrDefault();
			}
		}


		private ClaimsPrincipal getPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			try
			{

				var tokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = false,
					ValidateLifetime = true
				};

				var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
				if (!isJwtWithValidSecurityAlgorithm(validatedToken))
				{
					return null;
				}

				return principal;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
			
		}


		private bool isJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
		{
			return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
				jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
		}

		public async Task<UserRequest> RegisterAsync(UserRequest userRequest)
		{
			//var existingUser = await _userManager.FindByEmailAsync(email);

			//if (existingUser != null)
			//{
			//	return null;
			//}

			//var newUser = new IdentityUser()
			//{
			//	Email = email,
			//	UserName = email,
			//};

			//var createdUser = await _userManager.CreateAsync(newUser, password);

			//if (!createdUser.Succeeded)
			//{
			//	return null;
			//}


			string sql = "exec [AddUser] @FirstName, @LastName, @Password, @EmailAddress, @DateCreated";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, new {
					EmailAddress = userRequest.EmailAddress,
					DateCreated = DateTime.UtcNow,
					FirstName = userRequest.FirstName,
					LastName = userRequest.LastName,
					Password = userRequest.Password,
				});

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

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			
			byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
					new Claim(JwtRegisteredClaimNames.Sub, email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Email, email),
					new Claim("id", loggedInUser.Id.ToString())
				}),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			var refreshToken = new RefreshToken
			{
				Token = tokenHandler.WriteToken(token),
				JwtId = token.Id,
				UserId = loggedInUser.Id.ToString(),
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
			var validatedToken = getPrincipalFromToken(token);

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

			User currentUser = await getUserByIdAsync(int.Parse(validatedToken.Claims.Single(x => x.Type == "id").Value));

			if (currentUser == null)
			{
				return null;
			}
			else return await LoginAsync(currentUser.EmailAddress, currentUser.Password);
		}
	}
}
