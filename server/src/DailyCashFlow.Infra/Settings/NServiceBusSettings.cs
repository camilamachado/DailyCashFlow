namespace DailyCashFlow.Infra.Settings
{
	/// <summary>
	/// Classe que agrupa as configurações gerais do NServiceBus.
	/// </summary>
	public class NServiceBusSettings
	{
		/// <summary>
		/// Configurações relacionadas ao transporte do RabbitMQ.
		/// </summary>
		public RabbitMQSettings RabbitMQ { get; set; }

		/// <summary>
		/// Configurações de recuperabilidade para tratamento de falhas durante o processamento de mensagens.
		/// </summary>
		public RecoverabilitySettings Recoverability { get; set; } = new RecoverabilitySettings();
	}

	/// <summary>
	/// Classe que contém a string de conexão usada para se comunicar com o RabbitMQ.
	/// </summary>
	public class RabbitMQSettings
	{
		/// <summary>
		/// A string de conexão usada para configurar a conexão com o RabbitMQ.
		/// </summary>
		public string ConnectionString { get; set; }
	}

	/// <summary>
	/// Configurações relacionadas à recuperabilidade, que determinam como o NServiceBus lida com falhas de processamento de mensagens.
	/// </summary>
	public class RecoverabilitySettings
	{
		/// <summary>
		/// Define o número de tentativas imediatas que o NServiceBus fará para processar uma mensagem após uma falha.
		/// Valor padrão: 3.
		/// </summary>
		public int ImmediateRetries { get; set; } = 3;

		/// <summary>
		/// Define o número de tentativas com atraso que o NServiceBus fará se todas as tentativas imediatas falharem.
		/// Valor padrão: 2.
		/// </summary>
		public int DelayedRetries { get; set; } = 2;

		/// <summary>
		/// Define o intervalo de tempo entre as tentativas com atraso, caso o processamento da mensagem falhe.
		/// Valor padrão: 5 segundos.
		/// </summary>
		public TimeSpan TimeIncrease { get; set; } = TimeSpan.FromSeconds(5);
	}
}
