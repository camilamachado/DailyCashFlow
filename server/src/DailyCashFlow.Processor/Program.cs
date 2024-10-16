﻿using DailyCashFlow.Processor.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
	public static async Task Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((context, config) =>
			{
				config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
				config.AddEnvironmentVariables();
			})
			.ConfigureServices((context, services) =>
			{
				services.AddDatabaseConfiguration(context.Configuration);
				services.AddAutoMapperConfiguration();
				services.AddDependencyInjectionConfiguration();
				services.AddMediatrConfiguration();
				services.AddFluentValidationConfiguration();
			})
			.UseNServiceBus(context =>
			{
				var endpointConfiguration = new EndpointConfiguration("DailyCashFlow.Processor");

				context.AddNServiceBusConfiguration(endpointConfiguration);

				return endpointConfiguration;
			})
			.Build();

		await host.RunAsync();
	}
}
