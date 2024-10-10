using DailyCashFlow.Application;

namespace DailyCashFlow.Auth.Configurations
{
	public static class MediatrConfig
	{
		public static void AddMediatrConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssemblyContaining(typeof(Startup));
				cfg.RegisterServicesFromAssemblyContaining(typeof(AppModule));
			});
		}
	}
}
