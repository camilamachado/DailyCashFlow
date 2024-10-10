using DailyCashFlow.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Common.Tests.Configurations
{
	public static class DatabaseTestsConfig
	{
		public static void AddDatabaseTestsConfiguration(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			// Remove a configuração existente do DailyCashFlowDbContext
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType == typeof(DbContextOptions<DailyCashFlowDbContext>));

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			// Adiciona o DbContext usando a conexão de testes
			services.AddDbContext<DailyCashFlowDbContext>(options =>
			{
				options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DailyCashFlowIntegrationTestsDB;Trusted_Connection=True;MultipleActiveResultSets=true");
			});

			// Aplica as migrações no banco de dados de teste
			var serviceProvider = services.BuildServiceProvider();
			using (var scope = serviceProvider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DailyCashFlowDbContext>();
				dbContext.Database.Migrate();
			}
		}
	}
}
