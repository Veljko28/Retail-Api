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
	public class InventoryRepository : IInventoryRepository
	{
		private readonly IConfiguration configuration;

		public InventoryRepository(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public async Task<Inventory> addToInventoryAsync(InventoryRequest request)
		{
			using (SqlConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				string sql = "INSERT INTO Inventory (ProductId,Quantity,PurchasePrice,PurchaseDate) VALUES (@ProductId,@Quantity,@PurchasePrice,@PurchaseDate)";
				int rowsModified = await db.ExecuteAsync(sql, request);
				if (rowsModified > 0)
				{
					string getInv = "SELECT * FROM Inventory WHERE Id = (SELECT MAX(Id) FROM Inventory)";
					Inventory inv = (await db.QueryAsync<Inventory>(getInv)).FirstOrDefault();
					return inv;
				}
				else return null;
			}
		}

		public async Task<IEnumerable<Inventory>> getAllInInventoryAsync()
		{
			using (SqlConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				string sql = "SELECT * FROM Inventory";
				return await db.QueryAsync<Inventory>(sql);
			}
		}

		public async Task<Inventory> getByIdAsync(int id)
		{
			using (SqlConnection db = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();
				string sql = "SELECT * FROM Inventory WHERE Id = @Id";
				return (await db.QueryAsync<Inventory>(sql, new {Id = id } )).FirstOrDefault();
			}
		}
	}
}
