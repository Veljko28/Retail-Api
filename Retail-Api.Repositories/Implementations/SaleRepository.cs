using Dapper;
using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using Retail_Api.Models.Services;
using Retail_Api.Models.Requests;
using Retail_Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
			string sql = "exec [AddSale] @CashierId, @SaleDate, @SubTotal, @Tax, @Total";

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
			string sql = "exec [GetSales]";

			return await DbConnection.GenericConnectionAll<Sale>(sql, _configuration);
		}

		public async Task<Sale> getByIdAsync(int id)
		{
			string sql = "exec [GetSaleById] @Id";

			return await DbConnection.GenericConnectionID<Sale>(id, sql, _configuration);
		}

		public async Task<IEnumerable<Sale>> getSalesByDateAsync(string date)
		{
			string sql = "exec [GetSaleByDate] @SaleDate";
			
			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var sales = await db.QueryAsync<Sale>(sql, new { SaleDate = date });
				return sales;
			}
		}
	}
}
