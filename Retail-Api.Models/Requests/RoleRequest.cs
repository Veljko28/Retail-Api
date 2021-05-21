using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class RoleRequest
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string RoleId { get; set; }
	}
}
