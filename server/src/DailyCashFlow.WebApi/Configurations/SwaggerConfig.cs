using DailyCashFlow.WebApi.Filters;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
				Title = "DailyCashFlow.WebApi",
				Description = "Esta API é responsável por se comunicar com o front-end",
				Contact = new OpenApiContact { Name = "Camila Melo Machado", Url = new Uri("https://github.com/camilamachado") }
			});

			c.CustomSchemaIds(type =>
			{
				return type.FullName?.Replace("+", ".") ?? type.Name;
			});

			c.SchemaFilter<UniqueSchemaNameFilter>();
			c.DocumentFilter<IgnoreODataDocumentFilter>();

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);

			c.IgnoreObsoleteActions();
			c.IgnoreObsoleteProperties();
			c.DocInclusionPredicate((docName, apiDesc) =>
			{
				// Ignorar o endpoint de metadados e outros endpoints relacionados ao OData
				return !apiDesc.RelativePath.StartsWith("odata/$metadata") &&
					   !apiDesc.RelativePath.StartsWith("odata");
			});
		});
	}
}