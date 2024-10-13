using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Application.Features.Users.Handlers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DailyCashFlow.WebApi.Filters
{
	public class UniqueSchemaNameFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type == typeof(CategoryCreate.Command))
			{
				schema.Title = "CreateCategoryCommand"; // Renomeia o schema para evitar conflitos
			}
			else if (context.Type == typeof(UserCreate.Command))
			{
				schema.Title = "CreateUserCommand"; // Renomeia o schema para evitar conflitos
			}
		}
	}

}
