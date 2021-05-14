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
using Microsoft.AspNetCore.Authorization;

namespace Retail_Api.Controllers
{
	public class AccountController : Controller
	{
		private readonly IIdentityService _identity;


		public AccountController(IIdentityService identity)
		{
			_identity = identity;
		}


		public IActionResult genericResponse<T>(string errorMessage, T response)
		{
			if (response == null)
			{
				return BadRequest(errorMessage);
			}

			return Ok(response);
		}

		[HttpPost(Routes.AccountRoutes.Register)]
		public async Task<IActionResult> Register([FromBody] UserRequest request)
		{
			var response = await _identity.RegisterAsync(request);

			return genericResponse("Cannot register this user", response);
		}


		[HttpPost(Routes.AccountRoutes.Login)]
		public async Task<IActionResult> Login([FromBody] LoginInfo info)
		{
			var response = await _identity.LoginAsync(info.Email, info.Password);

			return genericResponse("Invalid login info", response);
		}


		[HttpPost(Routes.AccountRoutes.Refresh)]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
		{
			var response = await _identity.RefreshTokenAsync(request.Token, request.RefreshToken);

			return genericResponse("Cannot refresh token", response);
		} 

	}
}
