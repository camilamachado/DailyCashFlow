using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Domain.Features.DailyBalances
{
	public interface IDailyBalanceFactory
	{
		/// <summary>
		/// Cria um registro de saldo diário aplicando regras de negócio.
		/// </summary>
		Result<DailyBalance, Exception> Create(Transaction transaction);
	}

	public class DailyBalanceFactory : IDailyBalanceFactory
	{
		private readonly ILogger<DailyBalanceFactory> _logger;

		public DailyBalanceFactory(ILogger<DailyBalanceFactory> logger)
		{
			_logger = logger;
		}

		public Result<DailyBalance, Exception> Create(Transaction transaction)
		{
			var dailyBalance = new DailyBalance();
			dailyBalance.Date = transaction.Date;

			if (transaction.Type == TransactionType.Credit)
			{
				dailyBalance.AddCredit(transaction.Amount);

				return dailyBalance;
			}
			else
			{
				dailyBalance.AddDebit(transaction.Amount);

				return dailyBalance;
			}
		}
	}
}
