using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		public SaleController(ISaleRepository sales)
		{
			_sales = sales;
		}


		public IActionResult genericResponse<T>(string errorMessage, T response)
		{
			if (response == null)
			{
				return BadRequest(errorMessage);
			}

			return Ok(response);
		}

		[HttpGet(Routes.SaleRoutes.All)]
		public async Task<IActionResult> All()
		{
			IEnumerable<Sale> sales = await _sales.getAllAsync();
			
			return genericResponse("Cannot find any sales", sales);
		}


		[HttpGet(Routes.SaleRoutes.GetByDate)]
		public async Task<IActionResult> GetByDate(string date)
		{
			IEnumerable<Sale> sales = await _sales.getSalesByDateAsync(date);

			return genericResponse("Cannot find any sales created at " + date, sales);
		}

		[HttpGet(Routes.SaleRoutes.GetById)]
		public async Task<IActionResult> GetById(int saleId)
		{
			Sale sale = await _sales.getByIdAsync(saleId);

			return genericResponse("Cannot find sale with id :" + saleId, sale);
		}

		[HttpPost(Routes.SaleRoutes.CreateSale)]
		[Authorize(Policy = "Managment")]
		public async Task<IActionResult> Create([FromBody] SaleRequest saleReq)
		{

			Sale sale = await _sales.createAsync(saleReq);

			return genericResponse("Cannot create the sale", sale);
		}



	}
}
