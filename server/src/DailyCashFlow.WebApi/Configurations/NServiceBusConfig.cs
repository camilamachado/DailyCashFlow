using DailyCashFlow.Application.Features.Transactions.Events;
using DailyCashFlow.Infra.Settings;

namespace DailyCashFlow.WebApi.Configurations
{
	public static class NServiceBusConfig
	{
		public static void AddNServiceBusConfiguration(this HostBuilderContext context, EndpointConfiguration endpointConfiguration)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			var nserviceBusSettings = new NServiceBusSettings();
			context.Configuration.GetSection("NServiceBus").Bind(nserviceBusSettings);
			if (string.IsNullOrEmpty(nserviceBusSettings.RabbitMQ.ConnectionString))
				throw new InvalidOperationException("The ConnectionString for RabbitMQ must be configured..");
			
			// Configura o transporte
			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.ConnectionString(nserviceBusSettings.RabbitMQ.ConnectionString);
			transport.UseConventionalRoutingTopology(queueType: QueueType.Classic);

			// Configura eventos roteados
			transport.Routing().RouteToEndpoint(typeof(TransactionCreatedEvent), "DailyCashFlow.Processor");

			// Configura a serialização usando Newtonsoft.Json
			endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

			// Habilita criação automática de filas
			endpointConfiguration.EnableInstallers();
		}
	}
}
