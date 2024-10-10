using DailyCashFlow.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.WebApi.Configurations
{
	public static class DatabaseConfig
	{
		public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddDbContext<DailyCashFlowDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			var serviceProvider = services.BuildServiceProvider();

			var dailyCashFlowDbContext = serviceProvider.GetRequiredService<DailyCashFlowDbContext>();
			dailyCashFlowDbContext.Database.Migrate();
		}
	}
}
