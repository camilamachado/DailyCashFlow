namespace DailyCashFlow.Domain.Features.DailyBalances
{
	public class DailyBalance : BasicEntity
	{
		public DateTime Date { get; set; }
		public decimal TotalCredit { get; private set; }
		public decimal TotalDebit { get; private set; }
		public decimal NetBalance { get; private set; }

		public void AddCredit(decimal amount)
		{
			if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
			TotalCredit += amount;
			UpdateNetBalance();
		}

		public void AddDebit(decimal amount)
		{
			if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
			TotalDebit += amount;
			UpdateNetBalance();
		}

		private void UpdateNetBalance()
		{
			NetBalance = TotalCredit - TotalDebit;
		}
	}
}
