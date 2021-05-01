using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models
{
	public class User
	{
		[Required]
		[MaxLength(128)]
		public string Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(256)]
		public string EmailAdress { get; set; }

		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	}
}
