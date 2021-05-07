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
	public class SaleDetailController : Controller
	{
		private readonly ISaleDetailRepository _saleDetails;
		public SaleDetailController(ISaleDetailRepository saleDetails)
		{
			_saleDetails = saleDetails;
		}

		[HttpGet(Routes.SaleDetail.All)]
		public async Task<IActionResult> All()
		{
			IEnumerable<SaleDetail> sales = await _saleDetails.getAllAsync();

			if (sales.FirstOrDefault() == null)
			{
				return BadRequest("Cannot find any sales details");
			}

			return Ok(sales);
		}


		[HttpPost(Routes.SaleDetail.CreateDetail)]
		public async Task<IActionResult> Create([FromBody] SaleDetailRequest saleReq)
		{
			SaleDetail sale = await _saleDetails.createAsync(saleReq);

			if (sale == null)
			{
				return BadRequest("Cannot create the sale dtail");
			}

			return Ok(sale);
		}


		[HttpGet(Routes.SaleDetail.GetById)]
		public async Task<IActionResult> GetById(int saleDetailId)
		{
			SaleDetail sale = await _saleDetails.getByIdAsync(saleDetailId);

			if (sale == null)
			{
				return BadRequest("Cannot find sale detail with id :" + saleDetailId);
			}

			return Ok(sale);
		}


	}
}
