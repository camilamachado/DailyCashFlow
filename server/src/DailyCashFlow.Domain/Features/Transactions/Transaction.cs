using DailyCashFlow.Domain.Features.Categories;

namespace DailyCashFlow.Domain.Features.Transactions
{
	public class Transaction : BasicEntity
	{
		public int CategoryId { get; set; }
		public DateTime Date { get; set; }
		public TransactionType Type { get; set; }
		public decimal Amount { get; set; }
		public string Description { get; set; }
		public bool IsCancelled { get; private set; }
		public virtual Category Category { get; set; }

		public Transaction()
		{
			IsCancelled = false;
		}

		public void Cancel()
		{
			IsCancelled = true;
		}
	}
}
