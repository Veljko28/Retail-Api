using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Helpers
{
	public static class Routes
	{
		public const string Version = "/api/v1";

		public static class ProductRoutes
		{
			public const string RouteType = "/product";

			public const string GetAll = Version + "/products";

			public const string GetById = Version + RouteType + "/{productId}";

			public const string GetByName = Version + RouteType + "/name/{productName}";

			public const string Add = Version + RouteType + "/add";

			public const string DeleteById = Version + RouteType + "/{productId}";

			public const string UpdateById = Version + RouteType + "/{productId}";

		}
		public static class AccountRoutes
		{
			public const string Login = Version + "/login";

			public const string Register = Version + "/register";

			public const string Refresh = Version + "/refresh";

		}

		public static class SaleRoutes
		{
			public const string RouteType = "/sale";

			public const string CreateSale = Version + RouteType + "/create";

			public const string All = Version + RouteType + "/all";

			public const string GetByDate = Version + RouteType + "/date";

			public const string GetById = Version + RouteType + "/{saleId}";

		}

		public static class SaleDetailRoutes
		{
			public const string RouteType = "/saledetail";

			public const string CreateDetail = Version + RouteType + "/create";

			public const string All = Version + RouteType + "/all";

			public const string GetById = Version + RouteType + "/{saleId}";
		}



		public static class InventoryRoutes
		{
			public const string RouteType = "/inventory";

			public const string All = Version + RouteType + "/all";

			public const string Add = Version + RouteType + "/add";

			public const string GetById = Version + RouteType + "/{invId}";

		}

		public static class RolesRoutes 
		{
			public const string RouteType = "/roles";

			public const string GetUserRoles = Version + RouteType + "/{userId}";

			public const string UserRole = Version + RouteType;

			public const string GetRoleId = Version + RouteType + "/getId/{roleName}";

		}

	}
}
