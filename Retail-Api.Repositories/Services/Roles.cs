using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Services
{
	public static class Roles
	{
		public static async Task<IEnumerable<Role>> getUserRoles(string UserId, IConfiguration _configuration)
		{
			string sql = "SELECT * FROM UserRoles WHERE UserId = @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				IEnumerable<Role> roles = await db.QueryAsync<Role>(sql, new { UserId });

				return roles;
			}
		}


		public static async Task<string> getRoleName(string Id, IConfiguration _configuration)
		{
			string sql = "SELECT Name FROM Roles WHERE Id = @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				string roleName = (await db.QueryAsync<string>(sql, new { Id })).FirstOrDefault();

				return roleName;
			}
		}
	}
}
