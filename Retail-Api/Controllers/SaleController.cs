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
	public class SaleController : Controller
	{
		private readonly ISaleRepository _sales;
		private readonly IConfiguration _configuration;

		public SaleController(ISaleRepository sales, IConfiguration configuration)
		{
			_sales = sales;
			_configuration = configuration;
		}



		[HttpGet(Routes.SaleRoutes.All)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> All()
		{
			IEnumerable<Sale> sales = await _sales.getAllAsync();

			if (sales.FirstOrDefault() == null)
			{
				return BadRequest("Cannot find any sales");
			}

			return Ok(sales);
		}


		[HttpGet(Routes.SaleRoutes.GetByDate)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetByDate(string date)
		{
			IEnumerable<Sale> sales = await _sales.getSalesByDateAsync(date);

			if (sales == null)
			{
				return BadRequest("Cannot find any sales created at " + date);
			}

			return Ok(sales);
		}

		[HttpPost(Routes.SaleRoutes.CreateSale)]
		[Authorize(Policy = "Admin,Managment")]
		public async Task<IActionResult> Create([FromBody] SaleRequest saleReq)
		{
			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration) && !CheckRole.IsInRole(authorization, "Manager", _configuration))
			{
				return BadRequest("You don't have permission to add sales");
			}

			Sale sale = await _sales.createAsync(saleReq);

			if (sale == null)
			{
				return BadRequest("Cannot create the sale");
			}

			return Ok(sale);
		}


		[HttpGet(Routes.SaleRoutes.GetById)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetById(int saleId)
		{
			Sale sale = await _sales.getByIdAsync(saleId);

			if (sale == null)
			{
				return BadRequest("Cannot find sale with id :" + saleId);
			}

			return Ok(sale);
		}

	}
}
