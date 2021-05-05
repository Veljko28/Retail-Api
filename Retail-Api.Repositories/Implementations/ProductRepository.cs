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
	public class ProductRepository : IProductRepository
	{
		private readonly IConfiguration _configuration;

		public ProductRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		public async Task<Product> addAsync(ProductRequest entity)
		{
			string sql = "INSERT INTO Product (ProductName, Description, RetailPrice, CreateDate, LastModified)" +
				" VALUES (@ProductName, @Description, @RetailPrice, @CreateDate, @LastModified)";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, entity);

				string getProduct = "SELECT * FROM Product WHERE Id=(SELECT max(Id) FROM Product)";

				var products = await db.QueryAsync<Product>(getProduct, entity);
				return products.FirstOrDefault();
			}
		}

		public async Task<bool> deleteAsync(int id)
		{
			string sql = "DELETE FROM Product WHERE Id = @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int rowsModified = await db.ExecuteAsync(sql, new { Id = id });
				return rowsModified > 0;
			}
		}

		public async Task<IEnumerable<Product>> getAllAsync()
		{

			string sql = "SELECT * FROM Product";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				IEnumerable<Product> products = await db.QueryAsync<Product>(sql);
				return products;
			}
		}

		public async Task<Product> getByIdAsync(int id)
		{
			string sql = "SELECT * FROM Product WHERE Id = @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				var products = await db.QueryAsync<Product>(sql, new { Id = id });
				return products.FirstOrDefault();
			}
		}


		public async Task<IEnumerable<Product>> getByNameAsync(string Name)
		{
			string sql = "SELECT * FROM Product WHERE ProductName LIKE @ProductName";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				var products = await db.QueryAsync<Product>(sql, new { ProductName = '%' + Name + '%' });
				return products;
			}
		}

		public async Task<Product> updateAsync(Product entity)
		{
			string sql = "UPDATE Product SET ProductName = @ProductName, Description = @Description, RetailPrice = @RetailPrice,  CreateDate = @CreateDate, LastModified = @LastModified WHERE Id = @Id";

			if (await getByIdAsync(entity.Id) == null)
			{
				return null;
			}

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				int modified = await db.ExecuteAsync(sql, new { entity.Id, entity.ProductName, entity.Description, entity.RetailPrice, entity.CreateDate, entity.LastModified });
				return entity;
			}
		}
	}
}
