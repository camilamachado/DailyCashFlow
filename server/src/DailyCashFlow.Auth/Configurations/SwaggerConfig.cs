using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DailyCashFlow.Auth.Configurations
{
	public static class SwaggerConfig
	{
		public static void AddSwaggerConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "DailyCashFlow.Auth",
					Description = "Esta API é responsável por prover tokens de acesso para DailyCashFlow.WebApi",
					Contact = new OpenApiContact { Name = "Camila Melo Machado", Url = new Uri("https://github.com/camilamachado") }
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}
	}
}
