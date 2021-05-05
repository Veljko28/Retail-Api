using Retail_Api.Models;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Interfaces
{
	public interface ISaleRepository
	{
		Task<Sale> createAsync(SaleRequest saleRequest);

		Task<IEnumerable<Sale>> getAllAsync();

		Task<IEnumerable<Sale>> getSalesByDateAsync(string date);
	}
}
