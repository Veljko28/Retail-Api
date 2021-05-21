using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers
{
	public class LoginInfo
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MaxLength(50)]
		public string  Password { get; set; }
	}
}
