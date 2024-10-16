using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Processor.Configurations
{
	public static class DependencyInjectionConfig
	{
		public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// Repositories
			
		}
	}
}
