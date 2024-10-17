using AutoMapper;
using DailyCashFlow.Application.Features.DailyBalances.Handlers;
using DailyCashFlow.Application.Features.Transactions.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Processor.Features.Transactions
{
	public class TransactionCreatedEventHandler : IHandleMessages<TransactionCreatedEvent>
	{
		private readonly ILogger<TransactionCreatedEventHandler> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public TransactionCreatedEventHandler(
			ILogger<TransactionCreatedEventHandler> logger,
			IMediator mediator,
			IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;
		}

		public async Task Handle(TransactionCreatedEvent message, IMessageHandlerContext context)
		{
			_logger.LogInformation($"Processing created transaction: Amount {message.Amount}, Date {message.Date}, Type {message.Type}");

			var command = _mapper.Map<CalculateDailyBalance.Command>(message);

			var result = await _mediator.Send(command, context.CancellationToken);

			if (result.IsFailure)
			{
				_logger.LogError(result.Failure, "Error processing daily balance calculation for Date {Date}", message.Date);
				throw result.Failure;
			}
		}
	}
}
