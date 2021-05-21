using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers.Queries
{
	public class PaginationQuery
	{
		public int PageNumber { get; set; }
		public int  PageSize { get; set; }

		public PaginationQuery()
		{
			PageNumber = 1;
			PageSize = 100;
		}

		public PaginationQuery(int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize > 100 ? 100 : pageSize;
		}
	}
}
