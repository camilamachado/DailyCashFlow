using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DailyCashFlow.WebApi.Filters
{
	public class IgnoreODataDocumentFilter : IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			foreach (var key in swaggerDoc.Components.Schemas.Keys.ToList())
			{
				if (key.StartsWith("Microsoft.OData"))
				{
					swaggerDoc.Components.Schemas.Remove(key);
				}
			}
		}
	}
}
