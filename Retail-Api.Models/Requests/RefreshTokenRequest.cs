using System;
using System.Collections.Generic;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class RefreshTokenRequest
	{
		public string Token { get; set; }

		public string RefreshToken { get; set; }
	}
}
