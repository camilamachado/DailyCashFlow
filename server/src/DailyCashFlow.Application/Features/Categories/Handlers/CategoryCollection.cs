using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Categories.Handlers
{
	public class CategoryCollection
	{
		public class Query : IRequest<Result<IQueryable<Category>, Exception>> { }

		public class Handler : IRequestHandler<Query, Result<IQueryable<Category>, Exception>>
		{
			private readonly ICategoryRepository _categoryRepository;
			private readonly ILogger<Handler> _logger;

			public Handler(ICategoryRepository categoryRepository, ILogger<Handler> logger)
			{
				_categoryRepository = categoryRepository;
				_logger = logger;
			}

			public Task<Result<IQueryable<Category>, Exception>> Handle(Query request, CancellationToken cancellationToken)
			{
				var categoriesResult = _categoryRepository.GetAll();

				if (categoriesResult.IsFailure)
				{
					_logger.LogError(categoriesResult.Failure, "Failed to retrieve category collection.");

					return Result<IQueryable<Category>, Exception>.Fail(categoriesResult.Failure).AsTask();
				}

				return Result<IQueryable<Category>, Exception>.Ok(categoriesResult.Success).AsTask();
			}
		}
	}
}
