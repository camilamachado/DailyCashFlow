using AutoMapper;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Infra.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.DailyBalances.Handlers
{
	public class CalculateDailyBalance
	{
		public class Command : IRequest<Result<DailyBalance, Exception>>
		{
			public DateTime Date { get; set; }
			public TransactionType Type { get; set; }
			public decimal Amount { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(a => a.Date)
					.NotEmpty()
					.LessThanOrEqualTo(DateTime.Now)
					.GreaterThanOrEqualTo(new DateTime(1900, 1, 1));

				RuleFor(x => x.Type)
					.NotEmpty()
					.IsInEnum();

				RuleFor(x => x.Amount)
					.NotEmpty()
					.GreaterThan(0)
					.LessThanOrEqualTo(10_000_000);
			}
		}

		public class Handler : IRequestHandler<Command, Result<DailyBalance, Exception>>
		{
			private readonly IDailyBalanceFactory _dailyBalanceFactory;
			private readonly IDailyBalanceRepository _dailyBalanceRepository;
			private readonly IMapper _mapper;
			private readonly ILogger<Handler> _logger;

			public Handler(
				IDailyBalanceFactory dailyBalanceFactory,
				IDailyBalanceRepository dailyBalanceRepository,
				IMapper mapper,
				ILogger<Handler> logger)
			{
				_dailyBalanceFactory = dailyBalanceFactory;
				_dailyBalanceRepository = dailyBalanceRepository;
				_mapper = mapper;
				_logger = logger;
			}

			public async Task<Result<DailyBalance, Exception>> Handle(Command request, CancellationToken cancellationToken)
			{
				var transaction = _mapper.Map<Transaction>(request);

				var dailyBalanceResult = await _dailyBalanceRepository.GetByDateAsync(request.Date);
				if (dailyBalanceResult.IsFailure)
				{
					_logger.LogError(dailyBalanceResult.Failure, "Failed to retrieve daily balance for date {Date}.", request.Date);
					return dailyBalanceResult.Failure;
				}

				var dailyBalance = dailyBalanceResult.Success;

				// Se não existe saldo diário, cria um novo
				if (dailyBalance == null)
				{
					var createResult = _dailyBalanceFactory.Create(transaction);
					if (createResult.IsFailure)
					{
						_logger.LogError(createResult.Failure, "Failed to create daily balance for Date {Date}", request.Date);
						return createResult.Failure;
					}

					var addResult = await _dailyBalanceRepository.AddAsync(createResult.Success);
					if (addResult.IsFailure)
					{
						_logger.LogError(addResult.Failure, "Failed to add daily balance to repository for Date {Date}", request.Date);
						return addResult.Failure;
					}

					return addResult.Success;
				}
				else
				{
					// Atualiza o saldo diário existente
					if (transaction.Type == TransactionType.Credit)
						dailyBalance.AddCredit(transaction.Amount);
					else
						dailyBalance.AddDebit(transaction.Amount);

					var updateResult = await _dailyBalanceRepository.UpdateAsync(dailyBalance);
					if (updateResult.IsFailure)
					{
						_logger.LogError(updateResult.Failure, "Failed to update daily balance to repository for Date {Date}", request.Date);
						return updateResult.Failure;
					}

					return updateResult.Success;
				}
			}

		}
	}
}
