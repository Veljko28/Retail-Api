using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Retail_Api.Helpers;
using Retail_Api.Models;
using Retail_Api.Models.Requests;
using Retail_Api.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Retail_Api.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administration")]
	public class RolesController : Controller
	{
		private readonly IConfiguration _configuration;

		public RolesController(IConfiguration configuration)
		{
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

		[HttpGet(Routes.RolesRoutes.GetUserRoles)]
		public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
		{

			IEnumerable<Role> userRoles = await RoleRepository.getUserRolesAsync(userId, _configuration);

			if (userRoles.FirstOrDefault() == null)
			{
				return BadRequest("Cannot find user roles");
			}

			List<string> roleNames = new List<string>();

			foreach (Role r in userRoles)
			{
				roleNames.Add(await RoleRepository.getRoleNameAsync(r.RoleId, _configuration));
			}

			return Ok(roleNames);
		}


		[HttpGet(Routes.RolesRoutes.GetRoleId)]
		public async Task<IActionResult> GetRoleId([FromRoute] string roleName)
		{
			string Id = await RoleRepository.getRoleIdAsync(roleName, _configuration);

			if (string.IsNullOrEmpty(Id))
			{
				return BadRequest("Cannot find the id of this role");
			}
			return Ok(Id);
		}


		[HttpDelete(Routes.RolesRoutes.UserRole)]
		public async Task<IActionResult> DeleteUserRole([FromBody] RoleRequest request)
		{
			bool deleted = await RoleRepository.deleteRoleFromUserAsync(request, _configuration);

			if (!deleted)
			{
				return BadRequest("Cannot delete role from this user");
			}

			return Ok(deleted);
		}


		[HttpPost(Routes.RolesRoutes.UserRole)]
		public async Task<IActionResult> AddUserRole([FromBody] RoleRequest request)
		{
			bool added = await RoleRepository.addRoleToUserAsync(request, _configuration);

			if (!added)
			{
				return BadRequest("Cannot add role to this user");
			}

			return Ok(added);
		}
	}
}
