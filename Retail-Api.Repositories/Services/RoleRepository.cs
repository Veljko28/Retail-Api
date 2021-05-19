using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Services
{
	public static class RoleRepository
	{

		public static async Task<string> getRoleNameAsync(string Id, IConfiguration _configuration)
		{
			string sql = "SELECT Name FROM [dbo].[Roles] WHERE Id = @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				string roleName = (await db.QueryAsync<string>(sql, new { Id })).FirstOrDefault();

				return roleName;
			}
		}


		public static async Task<string> getRoleIdAsync(string Name, IConfiguration _configuration)
		{
			string sql = "SELECT Id FROM [dbo].[Roles] WHERE Name = @Name";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				string roleName = (await db.QueryAsync<string>(sql, new { Name })).FirstOrDefault();

				return roleName;
			}
		}
		
		public static async Task<IEnumerable<Role>> getUserRolesAsync(string UserId, IConfiguration _configuration)
		{
			string sql = "SELECT * FROM [dbo].[UserRoles]  WHERE UserId = @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				IEnumerable<Role> roles = await db.QueryAsync<Role>(sql, new { UserId });

				return roles;
			}
		}

		public static async Task<bool> deleteRoleFromUserAsync(RoleRequest request, IConfiguration _configuration)
		{
			string sql = "DELETE FROM [dbo].[UserRoles] WHERE RoleId = @RoleId AND UserId = @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified= (await db.ExecuteAsync(sql, new { RoleId = request.RoleId, UserId = request.UserId }));

				return rowsModified > 0;
			}
		}

		public static async Task<bool> addRoleToUserAsync(RoleRequest request, IConfiguration _configuration)
		{
			string sql = "INSERT INTO [dbo].[UserRoles] (RoleId, UserId) VALUES (@RoleId, @UserId)";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = (await db.ExecuteAsync(sql, new { RoleId = request.RoleId, UserId = request.UserId }));

				return rowsModified > 0;
			}
		}
	}
}
