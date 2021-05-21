using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers
{
	public class TokenResponse
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public string Expires { get; set; }
		[Required]
		public string RefreshToken { get; set; }
	}
}
