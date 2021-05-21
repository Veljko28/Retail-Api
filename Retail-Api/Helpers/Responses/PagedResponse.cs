using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers.Responses
{
	public class PagedResponse<T>
	{
		public IEnumerable<T> Data { get; set; }
		public int? PageNumber { get; set; }
		public int? PageSize{ get; set; }
		public string NextPage { get; set; }
		public string PreviousPage { get; set; }

		public PagedResponse(){}

		public PagedResponse(IEnumerable<T> data)
		{
			Data = data;
		}
	}
}
