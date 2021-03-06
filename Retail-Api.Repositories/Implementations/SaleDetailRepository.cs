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
	public class SaleDetailRepository : ISaleDetailRepository
	{
		private readonly IConfiguration _configuration;
		public SaleDetailRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<SaleDetail> createAsync(SaleDetailRequest saleRequest)
		{
			string sql = "exec [AddSaleDetail] @SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, saleRequest);

				string sale = "SELECT * FROM SaleDetail WHERE Id=(SELECT max(Id) FROM SaleDetail)";

				var sales = await db.QueryAsync<SaleDetail>(sale);
				return sales.FirstOrDefault();
			}
		}

		public async Task<IEnumerable<SaleDetail>> getAllAsync()
		{
			string sql = "exec [GetSaleDetails]";

			return await DbConnection.GenericConnectionAll<SaleDetail>(sql, _configuration);
		}

		public async Task<SaleDetail> getByIdAsync(int id)
		{
			string sql = "exec [GetSaleDetailById] @Id";

			return await DbConnection.GenericConnectionID<SaleDetail>(id, sql, _configuration);
		}
	
	}
}
