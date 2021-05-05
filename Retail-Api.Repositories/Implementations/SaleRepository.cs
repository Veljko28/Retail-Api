using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using Retail_Api.Models.Requests;
using Retail_Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Implementations
{
	public class SaleRepository : ISaleRepository
	{
		private readonly IConfiguration _configuration;
		public SaleRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Sale> createAsync(SaleRequest saleRequest)
		{
			string sql = "INSERT INTO Sale (CashierId, SaleDate, SubTotal, Tax, Total)" +
		" VALUES (@CashierId, @SaleDate, @SubTotal, @Tax, @Total)";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, saleRequest);

				string sale = "SELECT * FROM Sale WHERE Id=(SELECT max(Id) FROM Sale)";

				var sales = await db.QueryAsync<Sale>(sale);
				return sales.FirstOrDefault();
			}
		}

		public async Task<IEnumerable<Sale>> getAllAsync()
		{
			string sql = "SELECT * FROM Sale";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var sales = await db.QueryAsync<Sale>(sql);
				return sales;
			}
		}

		public async Task<IEnumerable<Sale>> getSalesByDateAsync(string date)
		{
			string sql = "SELECT * FROM Sale WHERE SUBSTRING(CONVERT(VARCHAR(25), SaleDate, 126),0,11) = @SaleDate";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var sales = await db.QueryAsync<Sale>(sql, new { SaleDate = date });
				return sales;
			}
		}
	}
}
