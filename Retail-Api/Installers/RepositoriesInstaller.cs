using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Retail_Api.Models.Services;
using Retail_Api.Repositories.Implementations;
using Retail_Api.Repositories.Interfaces;
using Retail_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Installers
{
	public class RepositoriesInstaller : IInstaller
	{
		public void InstallServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddScoped<IIdentityService, IdentityService>();

			services.AddSingleton<IProductRepository, ProductRepository>();

			services.AddSingleton<ISaleRepository, SaleRepository>()
				.AddSingleton<ISaleDetailRepository, SaleDetailRepository>()
				.AddSingleton<IInventoryRepository, InventoryRepository>();


			services.AddSingleton<IRefreshTokenDb, RefreshTokenDb>();
		}
	}
}
