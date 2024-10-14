using DailyCashFlow.Domain.Features.Transactions;

namespace DailyCashFlow.Domain.Features.Categories
{
	public class Category : BasicEntity
	{
		public string Name { get; set; }

		public ICollection<Transaction> Transactions { get; set; }
	}
}
