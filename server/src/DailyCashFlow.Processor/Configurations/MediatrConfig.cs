using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Processor.Configurations
{
	public static class MediatrConfig
	{
		public static void AddMediatrConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
			});
		}
	}
}