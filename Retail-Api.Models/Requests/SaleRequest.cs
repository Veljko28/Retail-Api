using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class SaleRequest
	{
		[Required]
		public int CashierId { get; set; }

		[Required]
		public DateTime SaleDate { get; set; }

		[Required]
		public decimal SubTotal { get; set; }

		[Required]
		public decimal Tax { get; set; }

		[Required]
		public decimal Total { get; set; }
	}
}
