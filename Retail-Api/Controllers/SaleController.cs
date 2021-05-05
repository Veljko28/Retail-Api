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
	public class SaleController : Controller
	{
		private readonly ISaleRepository _sales;
		public SaleController(ISaleRepository sales)
		{
			_sales = sales;
		}


		[HttpGet(Routes.Sale.All)]
		public async Task<IActionResult> All()
		{
			IEnumerable<Sale> sales = await _sales.getAllAsync();

			if (sales.FirstOrDefault() == null)
			{
				return BadRequest("Cannot find any sales");
			}

			return Ok(sales);
		}


		[HttpGet(Routes.Sale.GetByDate)]
		public async Task<IActionResult> GetByDate(string date)
		{
			IEnumerable<Sale> sales = await _sales.getSalesByDateAsync(date);

			if (sales == null)
			{
				return BadRequest("Cannot find any sales created at " + date);
			}

			return Ok(sales);
		}

		[HttpPost(Routes.Sale.CreateSale)]
		public async Task<IActionResult> Create([FromBody] SaleRequest saleReq)
		{
			Sale sale = await _sales.createAsync(saleReq);

			if (sale == null)
			{
				return BadRequest("Cannot create the sale");
			}

			return Ok(sale);
		}
	}
}
