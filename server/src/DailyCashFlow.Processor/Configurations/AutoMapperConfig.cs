using DailyCashFlow.Application.Features.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Processor.Configurations
{
	public static class AutoMapperConfig
	{
		public static void AddAutoMapperConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(typeof(Program), typeof(MappingProfile));
		}
	}
}
