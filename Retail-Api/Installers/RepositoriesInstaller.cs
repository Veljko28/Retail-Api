using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Retail_Api.Repositories.Implementations;
using Retail_Api.Repositories.Interfaces;
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
			services.AddSingleton<IProductRepository, ProductRepository>();
		}
	}
}
