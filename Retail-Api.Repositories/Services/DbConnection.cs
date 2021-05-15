using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Models.Services
{
	public class DbConnection
	{
		public static async Task<T> GenericConnectionID<T>(int Id, string sql, IConfiguration _configuration)
		{

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				T result = (await db.QueryAsync<T>(sql, new { Id })).FirstOrDefault();

				return result;
			}

		}


		public static async Task<IEnumerable<T>> GenericConnectionAll<T>(string sql, IConfiguration _configuration)
		{
			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				IEnumerable<T> allOfType = await db.QueryAsync<T>(sql);
				return allOfType;
			}
		}
	}
}
