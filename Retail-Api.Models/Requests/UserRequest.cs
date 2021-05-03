using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class UserRequest
	{
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(50)]
		public string Password { get; set; }

		[Required]
		[MaxLength(256)]
		public string EmailAddress { get; set; }

		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	}
}
