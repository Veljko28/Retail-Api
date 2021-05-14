using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Services
{
	public class RefreshTokenDb : IRefreshTokenDb
	{
		private readonly IConfiguration _configuration;

		public RefreshTokenDb(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> addTokenAsync(RefreshToken refreshToken)
		{
			string sql = "exec [AddRefreshToken] @Token, @JwtId, @CreatedDate, @Expires, @Used, @Invalidated, @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, refreshToken);

				return rowsModified > 0;
			}
		}

		public async Task<RefreshToken> findTokenAsync(string Token)
		{
			string sql = "exec [FindRefreshToken] @Token";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var response = await db.QueryAsync<RefreshToken>(sql,new { Token });

				return response.FirstOrDefault();
			}
		}

		public async Task<bool> updateTokenAsync(RefreshToken refreshToken)
		{
			string sql = "exec [UpdateRefeshToken] @Token, @JwtId, @CreatedDate, @Expires, @Used, @Invalidated, @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql,refreshToken);

				return rowsModified > 0;
			}
		}
	}
}
