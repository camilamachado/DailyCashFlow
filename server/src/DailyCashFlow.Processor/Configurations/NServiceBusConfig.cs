using DailyCashFlow.Infra.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RecoverabilitySettings = DailyCashFlow.Infra.Settings.RecoverabilitySettings;

namespace DailyCashFlow.Processor.Configurations
{
	public static class NServiceBusConfig
	{
		public static void AddNServiceBusConfiguration(this HostBuilderContext context, EndpointConfiguration endpointConfiguration)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			var nserviceBusSettings = new NServiceBusSettings();
			context.Configuration.GetSection("NServiceBus").Bind(nserviceBusSettings);

			// Configura o transporte
			ConfigureTransport(endpointConfiguration, nserviceBusSettings.RabbitMQ.ConnectionString);

			// Configura as formas de recuperação
			ConfigureRecoverability(endpointConfiguration, nserviceBusSettings.Recoverability);

			// Configura filas
			ConfigureQueues(endpointConfiguration);

			// Configura a serialização usando Newtonsoft.Json
			endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
		}

		private static void ConfigureTransport(EndpointConfiguration endpointConfiguration, string rabbitMQConnectionString)
		{
			if (string.IsNullOrEmpty(rabbitMQConnectionString))
				throw new ArgumentNullException(nameof(rabbitMQConnectionString));

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.ConnectionString(rabbitMQConnectionString);
			transport.UseConventionalRoutingTopology(queueType: QueueType.Classic);
		}

		private static void ConfigureRecoverability(EndpointConfiguration endpointConfiguration, RecoverabilitySettings recoverabilitySettings)
		{
			if (recoverabilitySettings == null) throw new ArgumentNullException(nameof(recoverabilitySettings));

			var recoverability = endpointConfiguration.Recoverability();
			recoverability.Immediate(immediate => immediate.NumberOfRetries(recoverabilitySettings.ImmediateRetries));
			recoverability.Delayed(delayed => delayed.NumberOfRetries(recoverabilitySettings.DelayedRetries)
													 .TimeIncrease(recoverabilitySettings.TimeIncrease));
		}

		private static void ConfigureQueues(EndpointConfiguration endpointConfiguration)
		{
			// Habilita criação automática de filas
			endpointConfiguration.EnableInstallers();

			// Configura a persistência das mensagens em memória para fins de desenvolvimento/teste
			endpointConfiguration.UsePersistence<LearningPersistence>();

			endpointConfiguration.AuditProcessedMessagesTo("DailyCashFlow.Processor.Audit");

			endpointConfiguration.SendFailedMessagesTo("DailyCashFlow.Processor.Error");
		}
	}
}
