using DailyCashFlow.WebApi;
using DailyCashFlow.WebApi.Configurations;
using Serilog;

public class Program
{
	public static void Main(string[] args)
	{
		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build())
			.CreateLogger();

		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			})
		.UseNServiceBus(context =>
		{
			var endpointConfiguration = new EndpointConfiguration("DailyCashFlow.WebApi");

			context.AddNServiceBusConfiguration(endpointConfiguration);

			return endpointConfiguration;
		});
}