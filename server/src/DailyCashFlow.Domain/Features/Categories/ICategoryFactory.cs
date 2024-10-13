using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Domain.Features.Categories
{
	public interface ICategoryFactory
	{
		/// <summary>
		/// Cria uma nova categoria
		/// </summary>
		Task<Result<Category, Exception>> CreateAsync(Category category);
	}

	public class CategoryFactory : ICategoryFactory
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly ILogger<CategoryFactory> _logger;

		public CategoryFactory(ICategoryRepository categoryRepository, ILogger<CategoryFactory> logger)
		{
			_categoryRepository = categoryRepository;
			_logger = logger;
		}

		public async Task<Result<Category, Exception>> CreateAsync(Category category)
		{
			var hasAnyCategoryCallback = await _categoryRepository.HasAnyByNameAsync(category.Name);
			if (hasAnyCategoryCallback.IsFailure)
			{
				_logger.LogError(hasAnyCategoryCallback.Failure, "Error checking if category {CategoryName} exists", category.Name);

				return hasAnyCategoryCallback.Failure;
			}
			else if (hasAnyCategoryCallback.Success)
			{
				return new AlreadyExistsException($"A category already exists with the name {category.Name}.");
			}

			return category;
		}
	}
}
