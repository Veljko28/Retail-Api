using Microsoft.AspNetCore.Mvc;
using Retail_Api.Helpers;
using Retail_Api.Models;
using Retail_Api.Models.Requests;
using Retail_Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Controllers
{
	public class ProductController : Controller
	{
		private IProductRepository _products;
		public ProductController(IProductRepository products)
		{
			_products = products;
		}

		[HttpGet(Routes.Products.GetAll)]
		public async Task<IActionResult> GetAll()
		{
			IEnumerable<Product> products = await _products.getAllAsync();
			if (products == null)
			{
				return NotFound("There are no product in the table");
			}

			return Ok(products);
		}


		[HttpGet(Routes.Products.GetById)]
		public async Task<IActionResult> GetById(int productId)
		{
			Product product = await _products.getByIdAsync(productId);
			if (product == null)
			{
				return NotFound("There is no product with id " + productId);
			}

			return Ok(product);
		}


		[HttpPost(Routes.Products.Add)]
		public async Task<IActionResult> Add([FromBody] ProductRequest entity)
		{
			var made = await _products.addAsync(entity);
			if (made == null)
			{
				return BadRequest("Invalid Product Input");
			}

			return Ok(made);
		}



		[HttpDelete(Routes.Products.DeleteById)]
		public async Task<IActionResult> Delete(int productId)
		{
			bool deleted = await _products.deleteAsync(productId);

			if (!deleted)
			{
				return BadRequest("Cannot find product with id " + productId);
			}

			return Ok("Succefully deleted product with Id: " + productId);
		}


		[HttpPatch(Routes.Products.UpdateById)]
		public async Task<IActionResult> Update([FromBody] ProductRequest entity, [FromRoute] int productId)
		{
			Product givenProduct = new Product
			{
				Id = productId,
				ProductName = entity.ProductName,
				Description = entity.Description,
				RetailPrice = entity.RetailPrice,
				CreateDate = entity.CreateDate,
				LastModified = DateTime.UtcNow
			};

			Product updatedProduct = await _products.updateAsync(givenProduct);

			if (updatedProduct == null)
			{
				return BadRequest("Error while trying to update the product");
			}

			return Ok(updatedProduct);
		}

	}
}
