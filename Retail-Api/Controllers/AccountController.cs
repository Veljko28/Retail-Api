using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Retail_Api.Helpers;
using Retail_Api.Models.Requests;
using Retail_Api.Models.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Retail_Api.Controllers
{
	public class AccountController : Controller
	{
		private readonly IIdentityService _identity;
		private readonly IConfiguration _configuration;


		public AccountController(IIdentityService identity, IConfiguration configuration)
		{
			_identity = identity;
			_configuration = configuration;
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

		[HttpGet("api/test/roles/{role}")]
		[Authorize(Policy = "Administration")]
		public IActionResult Roles([FromRoute] string role)
		{
			var authorization = Request.Headers[HeaderNames.Authorization];

			if (!CheckRole.IsInRole(authorization, role, _configuration))
			{
				return BadRequest("You are not in role: " + role);
			}

			return Ok("You are in role: " + role);
		}


	}
}
