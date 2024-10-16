using DailyCashFlow.Application.Features.Transactions.Events;
using Microsoft.Extensions.Logging;

public class TransactionCreatedEventHandler : IHandleMessages<TransactionCreatedEvent>
{
	private readonly ILogger<TransactionCreatedEventHandler> _logger;

	public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(TransactionCreatedEvent message, IMessageHandlerContext context)
	{
		_logger.LogInformation($"Processando transação criada: com valor {message.Amount}");

		return Task.CompletedTask;
	}
}
