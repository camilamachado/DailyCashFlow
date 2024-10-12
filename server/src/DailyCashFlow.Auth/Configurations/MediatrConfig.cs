using DailyCashFlow.Application.Features.Auth.Handlers;
using DailyCashFlow.Infra.ResultPattern;
using MediatR;

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
			});

			services.AddTransient<IRequestHandler<AuthToken.Command, Result<string, Exception>>, AuthToken.Handler>();
		}
	}
}
