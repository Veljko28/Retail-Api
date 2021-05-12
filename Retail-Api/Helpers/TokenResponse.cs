using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers
{
	public class TokenResponse
	{
		public string Token { get; set; }

		public string Expires { get; set; }

		public string RefreshToken { get; set; }
	}
}
