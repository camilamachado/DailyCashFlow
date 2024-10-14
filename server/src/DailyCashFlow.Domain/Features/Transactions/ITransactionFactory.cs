using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Domain.Features.Transactions
{
	public interface ITransactionFactory
	{
		/// <summary>
		/// Cria uma nova transação aplicando regras de negócio.
		/// </summary>
		Task<Result<Transaction, Exception>> CreateAsync(Transaction transaction);
	}

	public class TransactionFactory : ITransactionFactory
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ILogger<TransactionFactory> _logger;

		public TransactionFactory(ICategoryRepository categoryRepository, ITransactionRepository transactionRepository,  ILogger<TransactionFactory> logger)
		{
			_categoryRepository = categoryRepository;
			_transactionRepository = transactionRepository;
			_logger = logger;
		}

		public async Task<Result<Transaction, Exception>> CreateAsync(Transaction transaction)
		{
			var hasAnyCategoryCallback = await _categoryRepository.HasAnyByIdAsync(transaction.CategoryId);
			if (hasAnyCategoryCallback.IsFailure)
			{
				_logger.LogError(hasAnyCategoryCallback.Failure, "Error checking if category {CategoryId} exists", transaction.CategoryId);

				return hasAnyCategoryCallback.Failure;
			}
			else if (!hasAnyCategoryCallback.Success)
			{
				return new NotFoundException($"A category with ID {transaction.CategoryId} does not exist.");
			}

			var isDuplicateTransactionCallback = await _transactionRepository.HasDuplicateTransactionAsync(transaction);
			if (isDuplicateTransactionCallback.IsFailure)
			{
				_logger.LogError(isDuplicateTransactionCallback.Failure, "Error checking in repository if category is duplicate");

				return isDuplicateTransactionCallback.Failure;
			}
			else if (isDuplicateTransactionCallback.Success)
			{
				return new AlreadyExistsException($"A similar transaction already exists.");
			}

			return transaction;
		}
	}
}