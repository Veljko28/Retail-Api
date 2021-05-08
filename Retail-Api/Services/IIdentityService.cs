using Microsoft.IdentityModel.Tokens;
using Retail_Api.Helpers;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Models.Services
{
	public interface IIdentityService
	{
		Task<UserRequest> RegisterAsync(UserRequest userRequest);
		Task<TokenResponse> LoginAsync(string email, string password);
	}
}
