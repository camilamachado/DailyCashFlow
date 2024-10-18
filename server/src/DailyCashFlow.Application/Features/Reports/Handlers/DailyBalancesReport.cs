using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Infra.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Reports.Handlers
{
	public class DailyBalancesReport
	{
		public class Query : IRequest<Result<IQueryable<DailyBalance>, Exception>> { }

		public class Handler : IRequestHandler<Query, Result<IQueryable<DailyBalance>, Exception>>
		{
			private readonly IDailyBalanceRepository _dailyBalanceRepository;
			private readonly ILogger<Handler> _logger;

			public Handler(IDailyBalanceRepository dailyBalanceRepository, ILogger<Handler> logger)
			{
				_dailyBalanceRepository = dailyBalanceRepository;
				_logger = logger;
			}

			public Task<Result<IQueryable<DailyBalance>, Exception>> Handle(Query request, CancellationToken cancellationToken)
			{
				var categoriesResult = _dailyBalanceRepository.GetAllNoTracking();

				if (categoriesResult.IsFailure)
				{
					_logger.LogError(categoriesResult.Failure, "Failed to retrieve dailyBalance collection.");

					return Result<IQueryable<DailyBalance>, Exception>.Fail(categoriesResult.Failure).AsTask();
				}

				return Result<IQueryable<DailyBalance>, Exception>.Ok(categoriesResult.Success).AsTask();
			}
		}
	}
}
