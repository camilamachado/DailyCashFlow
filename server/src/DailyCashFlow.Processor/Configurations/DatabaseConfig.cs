using DailyCashFlow.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Processor.Configurations
{
	public static class DatabaseConfig
	{
		public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			var connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<DailyCashFlowDbContext>(options =>
				options.UseSqlServer(connectionString));
		}
	}
}
