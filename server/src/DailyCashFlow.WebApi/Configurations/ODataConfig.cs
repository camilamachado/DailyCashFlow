using DailyCashFlow.Domain.Features.Categories;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace DailyCashFlow.WebApi.Configurations
{
	public static class ODataConfig
	{
		public static void AddODataConfiguration(this ODataOptions oDataOptions, ODataConventionModelBuilder oDataBuilder)
		{
			if (oDataOptions == null) throw new ArgumentNullException(nameof(oDataOptions));
			if (oDataBuilder == null) throw new ArgumentNullException(nameof(oDataBuilder));

			oDataOptions.Select().Expand().Filter().OrderBy().SetMaxTop(100).Count();

			oDataOptions.EnableQueryFeatures()
				.Select().Count().Filter().OrderBy().Expand().SetMaxTop(100);

			// Adiciona a configuração OData sem o endpoint de metadados
			oDataOptions.AddRouteComponents("odata", oDataBuilder.GetEdmModel());
			oDataBuilder.EntitySet<Category>("Category");
		}
	}
}
