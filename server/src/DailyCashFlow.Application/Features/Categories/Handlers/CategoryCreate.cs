using AutoMapper;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Categories.Handlers
{
	public class CategoryCreate
	{
		public class Command : IRequest<Result<int, Exception>>
		{
			public string Name { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(a => a.Name).NotEmpty().Length(1, 50);
			}
		}

		public class Handler : IRequestHandler<Command, Result<int, Exception>>
		{
			private readonly ICategoryFactory _categoryFactory;
			private readonly ICategoryRepository _categoryRepository;
			private readonly IMapper _mapper;
			private readonly ILogger<Handler> _logger;

			public Handler(ICategoryFactory categoryFactory, ICategoryRepository categoryRepository, IMapper mapper, ILogger<Handler> logger)
			{
				_categoryFactory = categoryFactory;
				_categoryRepository = categoryRepository;
				_mapper = mapper;
				_logger = logger;
			}

			public async Task<Result<int, Exception>> Handle(Command request, CancellationToken cancellationToken)
			{
				var category = _mapper.Map<Category>(request);

				var createCategoryCallback = await _categoryFactory.CreateAsync(category);
				if (createCategoryCallback.IsFailure)
				{
					_logger.LogError(createCategoryCallback.Failure, "Failed to create category with name {CategoryName}", category.Name);
					return createCategoryCallback.Failure;
				}

				var addCategoryCallback = await _categoryRepository.AddAsync(createCategoryCallback.Success);
				if (addCategoryCallback.IsFailure)
				{
					_logger.LogError(addCategoryCallback.Failure, "Failed to add category to repository with name {CategoryName}", category.Name);
					return addCategoryCallback.Failure;
				}

				return addCategoryCallback.Success.Id;
			}
		}
	}
}
