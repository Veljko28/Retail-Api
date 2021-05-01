﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models
{
	public class Product
	{ 
		[Required]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string ProductName { get; set; }

		[Required]
		public Decimal RetailPrice { get; set; }

		[Required]
		public string Description { get; set; }
		[Required]
		public DateTime CreateDate { get; set; } = DateTime.UtcNow;

		[Required]
		public DateTime LastModified { get; set; } = DateTime.UtcNow;

	}
}
