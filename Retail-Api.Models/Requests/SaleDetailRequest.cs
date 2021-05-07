using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retail_Api.Models.Requests
{
	public class SaleDetailRequest
	{

		[Required]
		public int SaleId { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; } = 1;

		[Required]
		public decimal PurchasePrice { get; set; }

		[Required]
		public decimal Tax { get; set; } = 0;
	}
}
