using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
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
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ProductController : Controller
	{
		private IProductRepository _products;
		private readonly IConfiguration _configuration;

		public ProductController(IProductRepository products, IConfiguration configuration)
		{
			_products = products;
			_configuration = configuration;
		}

		[HttpGet(Routes.ProductRoutes.GetAll)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetAll()
		{
			IEnumerable<Product> products = await _products.getAllAsync();
			if (products == null)
			{
				return NotFound("There are no product in the table");
			}

			return Ok(products);
		}


		[HttpGet(Routes.ProductRoutes.GetById)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetById(int productId)
		{
			Product product = await _products.getByIdAsync(productId);
			if (product == null)
			{
				return NotFound("There is no product with id " + productId);
			}

			return Ok(product);
		}

		[HttpGet(Routes.ProductRoutes.GetByName)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetByName(string productName)
		{
			IEnumerable<Product> product = await _products.getByNameAsync(productName);
			if (product.FirstOrDefault() == null)
			{
				return NotFound("There is no product with name :" + productName);
			}

			return Ok(product);
		}


		[HttpPost(Routes.ProductRoutes.Add)]
		[Authorize(Policy = "Managment")]
		public async Task<IActionResult> Add([FromBody] ProductRequest entity)
		{
			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration) && !CheckRole.IsInRole(authorization, "Manager", _configuration))
			{
				return BadRequest("You don't have permission to add products");
			}

			var made = await _products.addAsync(entity);
			if (made == null)
			{
				return BadRequest("Invalid Product Input");
			}

			return Ok(made);
		}



		[HttpDelete(Routes.ProductRoutes.DeleteById)]
		[Authorize(Policy = "Admin,Managment")]
		public async Task<IActionResult> Delete(int productId)
		{

			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration) && !CheckRole.IsInRole(authorization, "Manager", _configuration))
			{
				return BadRequest("You don't have permission to delete products");
			}

			bool deleted = await _products.deleteAsync(productId);

			if (!deleted)
			{
				return BadRequest("Cannot find product with id " + productId);
			}

			return Ok("Succefully deleted product with Id: " + productId);
		}



		[HttpPatch(Routes.ProductRoutes.UpdateById)]
		[Authorize(Policy = "Admin,Managment")]
		public async Task<IActionResult> Update([FromBody] ProductRequest entity, [FromRoute] int productId)
		{

			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration) && !CheckRole.IsInRole(authorization, "Manager", _configuration))
			{
				return BadRequest("You don't have permission to update products");
			}

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
