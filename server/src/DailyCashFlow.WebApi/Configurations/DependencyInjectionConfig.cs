﻿using DailyCashFlow.Application.Features.Auth.Services;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.Data.Features.Categories;
using DailyCashFlow.Infra.Data.Features.DailyBalances;
using DailyCashFlow.Infra.Data.Features.Transactions;
using DailyCashFlow.Infra.Data.Features.Users;

namespace DailyCashFlow.WebApi.Configurations
{
	public static class DependencyInjectionConfig
	{
		public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// Services
			services.AddScoped<IJWTService, JWTService>();

			// Factories
			services.AddScoped<IUserFactory, UserFactory>();
			services.AddScoped<ICategoryFactory, CategoryFactory>();
			services.AddScoped<ITransactionFactory, TransactionFactory>();
			services.AddScoped<IDailyBalanceFactory, DailyBalanceFactory>();

			// Repositories
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IDailyBalanceRepository, DailyBalanceRepository>();
		}
	}
}

