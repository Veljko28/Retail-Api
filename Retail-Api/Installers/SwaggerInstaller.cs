using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Installers
{
	public class SwaggerInstaller : IInstaller
	{
		public void InstallServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Retail_Api", Version = "v1" });
			});
		}
	}
}
