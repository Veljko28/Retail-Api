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
	public class InventoryController : Controller
	{
		private readonly IInventoryRepository _inventory;
		private readonly IConfiguration _configuration;

		public InventoryController(IInventoryRepository inventory, IConfiguration configuration)
		{
			_inventory = inventory;
			_configuration = configuration;
		}

		[HttpGet(Routes.InventoryRoutes.All)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> All()
		{
			IEnumerable<Inventory> inv =  await _inventory.getAllInInventoryAsync();

			if (inv.FirstOrDefault() == null)
			{
				return NotFound("Cannot find any inventory");
			}
			return Ok(inv);
		}


		[HttpPost(Routes.InventoryRoutes.Add)]
		[Authorize(Policy = "Managment")]
		public async Task<IActionResult> Add([FromBody] InventoryRequest request)
		{
			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, "Admin", _configuration)){
				return BadRequest("You don't have permission to add to the inventory");
			}

			Inventory inv = await _inventory.addToInventoryAsync(request);

			if (inv == null)
			{
				return BadRequest("Cannot add this to the inventory");
			}
			return Ok(inv);
		}


		[HttpGet(Routes.InventoryRoutes.GetById)]
		[Authorize(Policy = "Admin,Managment,Cashier")]
		public async Task<IActionResult> GetById([FromRoute] int invId)
		{
			Inventory inv = await _inventory.getByIdAsync(invId);

			if (inv == null)
			{
				return NotFound("Cannot find this item in the inventory");
			}
			return Ok(inv);
		}
	}
}
