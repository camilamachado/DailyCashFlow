using AutoMapper;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Infra.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Transactions.Handlers
{
	public class TransactionCreate
	{
		public class Command : IRequest<Result<int, Exception>>
		{
			public int CategoryId { get; set; }
			public DateTime Date { get; set; }
			public TransactionType Type { get; set; }
			public decimal Amount { get; set; }
			public string Description { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(a => a.CategoryId)
					.NotNull()
					.NotEmpty()
					.GreaterThan(0);

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

				RuleFor(a => a.Description)
					.MaximumLength(255);
			}
		}

		public class Handler : IRequestHandler<Command, Result<int, Exception>>
		{
			private readonly ITransactionFactory _transactionFactory;
			private readonly ITransactionRepository _transactionRepository;
			private readonly IMapper _mapper;
			private readonly ILogger<Handler> _logger;

			public Handler(ITransactionFactory transactionFactory, ITransactionRepository transactionRepository, IMapper mapper, ILogger<Handler> logger)
			{
				_transactionFactory = transactionFactory;
				_transactionRepository = transactionRepository;
				_mapper = mapper;
				_logger = logger;
			}

			public async Task<Result<int, Exception>> Handle(Command request, CancellationToken cancellationToken)
			{
				var transaction = _mapper.Map<Transaction>(request);

				var createTransactionCallback = await _transactionFactory.CreateAsync(transaction);
				if (createTransactionCallback.IsFailure)
				{
					_logger.LogError(createTransactionCallback.Failure, "Failed to create transaction for CategoryId {CategoryId} and Date {Date}", transaction.CategoryId, transaction.Date);
					return createTransactionCallback.Failure;
				}

				var addTransactionCallback = await _transactionRepository.AddAsync(createTransactionCallback.Success);
				if (addTransactionCallback.IsFailure)
				{
					_logger.LogError(addTransactionCallback.Failure, "Failed to add transaction to repository for CategoryId {CategoryId} and Date {Date}", transaction.CategoryId, transaction.Date);
					return addTransactionCallback.Failure;
				}

				return addTransactionCallback.Success.Id;
			}
		}
	}
}