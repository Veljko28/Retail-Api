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
	public class SaleDetailController : Controller
	{
		private readonly ISaleDetailRepository _saleDetails;
		private readonly IConfiguration _configuration;
		public SaleDetailController(ISaleDetailRepository saleDetails, IConfiguration configuration)
		{
			_saleDetails = saleDetails;
			_configuration = configuration;
		}

		[HttpGet(Routes.SaleDetailRoutes.All)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> All()
		{
			IEnumerable<SaleDetail> sales = await _saleDetails.getAllAsync();

			if (sales.FirstOrDefault() == null)
			{
				return BadRequest("Cannot find any sales details");
			}

			return Ok(sales);
		}


		[HttpPost(Routes.SaleDetailRoutes.CreateDetail)]
		[Authorize(Policy = "Admin,Managment")]
		public async Task<IActionResult> Create([FromBody] SaleDetailRequest saleReq)
		{
			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration) && !CheckRole.IsInRole(authorization, "Manager", _configuration))
			{
				return BadRequest("You don't have permission to add sales");
			}

			SaleDetail sale = await _saleDetails.createAsync(saleReq);

			if (sale == null)
			{
				return BadRequest("Cannot create the sale dtail");
			}

			return Ok(sale);
		}


		[HttpGet(Routes.SaleDetailRoutes.GetById)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetById([FromRoute] int saleId)
		{
			SaleDetail sale = await _saleDetails.getByIdAsync(saleId);

			if (sale == null)
			{
				return BadRequest("Cannot find sale detail with id :" + saleId);
			}

			return Ok(sale);
		}


	}
}
