using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Retail_Api.Models
{
	public class RefreshToken
	{
		[Key]
		public string Token { get; set; }
		public string JwtId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime Expires { get; set; }
		public bool Used { get; set; }
		public bool  Invalidated { get; set; }
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User user { get; set; }
	}
}
