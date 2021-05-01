using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models
{
	public class Sale
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[MaxLength(128)]
		public string CashierId { get; set; }

		[Required]
		public DateTime SaleDate { get; set; }

		[Required]
		public Decimal SubTotal { get; set; }

		[Required]
		public Decimal Tax { get; set; }

		[Required]
		public Decimal Total { get; set; }

	}
}
