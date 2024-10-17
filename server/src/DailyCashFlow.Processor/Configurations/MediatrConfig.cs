using DailyCashFlow.Application.Features.DailyBalances.Handlers;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Infra.ResultPattern;
using MediatR;
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

			services.AddTransient<IRequestHandler<CalculateDailyBalance.Command, Result<DailyBalance, Exception>>, CalculateDailyBalance.Handler>();
		}
	}
}