using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Infra.Data.Features.DailyBalances;
using DailyCashFlow.Infra.Data.Features.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Processor.Configurations
{
	public static class DependencyInjectionConfig
	{
		public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// Factories
			services.AddScoped<IDailyBalanceFactory, DailyBalanceFactory>();

			// Repositories
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IDailyBalanceRepository, DailyBalanceRepository>();
		}
	}
}
