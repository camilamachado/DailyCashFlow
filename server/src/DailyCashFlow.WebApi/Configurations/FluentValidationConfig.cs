using DailyCashFlow.Application;
using DailyCashFlow.WebApi.Behaviors;
using FluentValidation;
using MediatR;

namespace DailyCashFlow.WebApi.Configurations
{
	public static class FluentValidationConfig
	{
		public static void AddFluentValidationConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// Registra o comportamento de validação
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

			// Obtém o assembly que contém os Validators
			var assembly = typeof(AppModule).Assembly;

			// Registra todos os validadores
			var validatorTypes = assembly.GetTypes()
				.Where(type => type.IsClass && !type.IsAbstract &&
							   type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)));

			foreach (var validatorType in validatorTypes)
			{
				var interfaceType = validatorType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));
				services.AddTransient(interfaceType, validatorType);
			}
		}
	}
}
