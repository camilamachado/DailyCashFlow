using DailyCashFlow.Application.Features.Auth.Services;
using DailyCashFlow.Domain.Features.Users;
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

			// Repositories
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserFactory, UserFactory>();
		}
	}
}

