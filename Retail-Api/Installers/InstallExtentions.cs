using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Installers
{
	public static class InstallExtentions
	{
		public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration Configuration)
		{
			List<IInstaller> installers = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
						.Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

			foreach (IInstaller installer in installers)
			{
				installer.InstallServices(Configuration, services);
			}
		}
	}
}
