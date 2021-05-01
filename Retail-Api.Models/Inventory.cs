using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models
{
	public class Inventory
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; } = 1;

		[Required]
		public Decimal PurchasePrice { get; set; }

		[Required]
		public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
	}
}
