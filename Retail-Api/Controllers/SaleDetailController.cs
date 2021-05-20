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
	public class SaleDetailController : Controller
	{
		private readonly ISaleDetailRepository _saleDetails;
		public SaleDetailController(ISaleDetailRepository saleDetails)
		{
			_saleDetails = saleDetails;
		}
		public IActionResult genericResponse<T>(string errorMessage, T response)
		{
			if (response == null)
			{
				return BadRequest(errorMessage);
			}

			return Ok(response);
		}

		[HttpGet(Routes.SaleDetailRoutes.All)]
		public async Task<IActionResult> All()
		{
			IEnumerable<SaleDetail> sales = await _saleDetails.getAllAsync();

			return genericResponse("Cannot find any sale details", sales.FirstOrDefault());
		}

		[HttpGet(Routes.SaleDetailRoutes.GetById)]
		public async Task<IActionResult> GetById([FromRoute] int saleId)
		{
			SaleDetail sale = await _saleDetails.getByIdAsync(saleId);

			return genericResponse("Cannot find sale detail with id :" + saleId, sale);
		}


		[HttpPost(Routes.SaleDetailRoutes.CreateDetail)]
		[Authorize(Policy = "Managment")]
		public async Task<IActionResult> Create([FromBody] SaleDetailRequest saleReq)
		{
			SaleDetail sale = await _saleDetails.createAsync(saleReq);

			return genericResponse("Cannot create the sale detail", sale);
		}

	}
}
