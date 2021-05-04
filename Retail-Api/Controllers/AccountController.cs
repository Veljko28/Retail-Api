using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Retail_Api.Helpers;
using Retail_Api.Models.Requests;
using Retail_Api.Models.Services;
using Retail_Api.Models;
using System.Text;
using Retail_Api.Repositories.Interfaces;

namespace Retail_Api.Controllers
{
	public class AccountController : Controller
	{
		private readonly IIdentityService _identity;


		public AccountController(IIdentityService identity)
		{
			_identity = identity;
		}

		[HttpPost(Routes.Account.Register)]
		public async Task<IActionResult> Register([FromBody] UserRequest request)
		{
			var response = await _identity.RegisterAsync(request);

			if (response == null)
			{
				return BadRequest("Cannot register this user");
			}

			return Ok(response);
		}


		[HttpPost(Routes.Account.Login)]
		public async Task<IActionResult> Login([FromBody] LoginInfo info)
		{
			var response = await _identity.LoginAsync(info.Email, info.Password);

			if (response == null)
			{
				return BadRequest("Invalid login info");
			}

			return Ok(response);
		}
	}
}
