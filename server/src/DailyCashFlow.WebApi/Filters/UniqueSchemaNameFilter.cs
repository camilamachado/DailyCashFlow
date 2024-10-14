using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.Application.Features.Users.Handlers;
using DailyCashFlow.Domain.Features.Transactions;
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
				schema.Title = "CreateUserCommand";
			}
			else if (context.Type == typeof(TransactionCreate.Command))
			{
				schema.Title = "CreateTransactionCommand";
			}
			else if (context.Type == typeof(TransactionType))
			{
				schema.Title = "TransactionTypeEnum";
			}
		}
	}

}
