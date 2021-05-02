using Retail_Api.Models;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retail_Api.Repositories.Interfaces
{
	public interface IProductRepository : IGenericInterface<Product, ProductRequest>
	{
	}
}
