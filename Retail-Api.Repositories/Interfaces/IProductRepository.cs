using Retail_Api.Models;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Interfaces
{
	public interface IProductRepository : IGenericInterface<Product, ProductRequest>
	{
		Task<IEnumerable<Product>> getByNameAsync(string Name);
	}
}
