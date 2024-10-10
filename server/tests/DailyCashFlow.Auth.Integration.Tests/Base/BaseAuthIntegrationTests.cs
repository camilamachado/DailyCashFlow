using DailyCashFlow.Common.Tests.Configurations;
using DailyCashFlow.Common.Tests.Helpers;
using DailyCashFlow.Infra.Data.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DailyCashFlow.Auth.Integration.Tests.Base
{
	public abstract class BaseAuthIntegrationTests : IDisposable
	{
		protected HttpClient _client;
		protected WebApplicationFactory<Startup> _factory;
		protected DailyCashFlowDbContext _dbContext;

		[SetUp]
		public void Setup()
		{
			_factory = new WebApplicationFactory<Startup>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureServices(services =>
					{
						services.AddDatabaseTestsConfiguration();
					});
				});

			_client = _factory.CreateClient();

			var scope = _factory.Services.CreateScope();
			_dbContext = scope.ServiceProvider.GetRequiredService<DailyCashFlowDbContext>();
		}

		[TearDown]
		public void Cleanup()
		{
			DatabaseCleanup.Cleanup(_dbContext);
		}

		public void Dispose()
		{
			_client?.Dispose();
			_factory?.Dispose();
			_dbContext?.Dispose();
		}
	}
}
