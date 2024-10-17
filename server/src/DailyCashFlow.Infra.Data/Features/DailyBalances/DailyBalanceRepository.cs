using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Infra.Data.Context;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Features.DailyBalances
{
	public class DailyBalanceRepository : IDailyBalanceRepository
	{
		private readonly DailyCashFlowDbContext _context;

		public DailyBalanceRepository(DailyCashFlowDbContext context)
		{
			_context = context;
		}

		public async Task<Result<DailyBalance, Exception>> AddAsync(DailyBalance dailyBalance)
		{
			_context.DailyBalances.Add(dailyBalance);
			await _context.SaveChangesAsync();

			return dailyBalance;
		}

		#nullable enable
		public async Task<Result<DailyBalance?, Exception>> GetByDateAsync(DateTime date)
		{
			var dailyBalance = await _context.DailyBalances.FirstOrDefaultAsync(u => u.Date.Date == date.Date);

			return dailyBalance; 
		}
		#nullable disable

		public async Task<Result<DailyBalance, Exception>> UpdateAsync(DailyBalance dailyBalance)
		{
			_context.DailyBalances.Update(dailyBalance);
			await _context.SaveChangesAsync();

			return dailyBalance;
		}
	}
}
