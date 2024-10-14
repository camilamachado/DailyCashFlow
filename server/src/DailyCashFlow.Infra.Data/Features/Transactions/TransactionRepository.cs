using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Infra.Data.Context;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Features.Transactions
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly DailyCashFlowDbContext _context;

		public TransactionRepository(DailyCashFlowDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Transaction, Exception>> AddAsync(Transaction transaction)
		{
			_context.Transactions.Add(transaction);
			await _context.SaveChangesAsync();

			return transaction;
		}

		public async Task<Result<bool, Exception>> HasDuplicateTransactionAsync(Transaction transaction)
		{
			var hasAny = await _context.Transactions
						.AnyAsync(t =>
							t.CategoryId == transaction.CategoryId &&
							t.Date == transaction.Date &&
							t.Type == transaction.Type &&
							t.Amount == transaction.Amount &&
							t.Description == transaction.Description &&
							t.IsCancelled == transaction.IsCancelled
						);

			return hasAny;
		}
	}
}
