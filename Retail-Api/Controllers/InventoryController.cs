using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class InventoryController : Controller
	{
		private readonly IInventoryRepository _inventory;

		public InventoryController(IInventoryRepository inventory)
		{
			_inventory = inventory;
		}

		[HttpGet(Routes.InventoryRoutes.All)]
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
		public async Task<IActionResult> Add([FromBody] InventoryRequest request)
		{
			Inventory inv = await _inventory.addToInventoryAsync(request);

			if (inv == null)
			{
				return BadRequest("Cannot add this to the inventory");
			}
			return Ok(inv);
		}


		[HttpGet(Routes.InventoryRoutes.GetById)]
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
