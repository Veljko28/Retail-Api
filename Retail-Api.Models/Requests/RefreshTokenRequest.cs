using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class RefreshTokenRequest
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public string RefreshToken { get; set; }
	}
}
