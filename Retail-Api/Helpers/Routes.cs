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

			public const string GetByName = Version + RouteType + "/name/{productName}";

			public const string Add = Version + RouteType + "/add";

			public const string DeleteById = Version + RouteType + "/{productId}";

			public const string UpdateById = Version + RouteType + "/{productId}";

		}
		public static class Account
		{
			public const string Login = Version + "/login";

			public const string Register = Version + "/register";

		}

		public static class Sale
		{
			public const string RouteType = "/sale";

			public const string CreateSale = Version + RouteType + "/create";

			public const string All = Version + RouteType + "/all";

			public const string GetByDate = Version + RouteType + "/date";

		}

	}
}
