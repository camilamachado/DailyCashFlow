using DailyCashFlow.Domain.Features.Transactions;

namespace DailyCashFlow.Application.Features.Transactions.Events
{
	public class TransactionCreatedEvent : IEvent
	{
		public DateTime Date { get; set; }
		public TransactionType Type { get; set; }
		public decimal Amount { get; set; }
	}
}
