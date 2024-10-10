using DailyCashFlow.Application.Features.Users;

namespace DailyCashFlow.Auth.Configurations
{
	public static class AutoMapperConfig
	{
		public static void AddAutoMapperConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(typeof(Startup), typeof(MappingProfile));
		}
	}
}
