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
		public IdentityService(JwtSettings jwtSettings, IConfiguration configuration)
		{
			_jwtSettings = jwtSettings;
			_configuration = configuration;
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


			string sql = "INSERT INTO [dbo].[User] ( FirstName, LastName, Password, EmailAddress, DateCreated) VALUES (@FirstName, @LastName, @Password, @EmailAddress, @DateCreated);";

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
		public async Task<string> LoginAsync(string email, string password)
		{
			User loggedInUser = new User();

			string sql = "SELECT * FROM [dbo].[User] WHERE EmailAddress = @Email AND Password = @Password";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				var result = await db.QueryAsync<User>(sql, new
				{
					Email = email,
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
				Expires = DateTime.UtcNow.AddDays(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
