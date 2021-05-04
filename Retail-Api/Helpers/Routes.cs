using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers
{
	public static class Routes
	{
		public const string Version = "/api/v1";

		public static class Products
		{
			public const string RouteType = "/product";

			public const string GetAll = Version + "/products";

			public const string GetById = Version + RouteType + "/{productId}";
			
			public const string Add = Version + RouteType + "/add";

			public const string DeleteById = Version + RouteType + "/{productId}";

			public const string UpdateById = Version + RouteType + "/{productId}";

		}
		public static class Account
		{
			public const string Login = Version + "/login";

			public const string Register = Version + "/register";

		}

	}
}
