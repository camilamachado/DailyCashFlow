using System.Text.Json.Serialization;

namespace DailyCashFlow.Domain.Features.Transactions
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TransactionType
	{
		Credit = 'C',
		Debit = 'D'
	}
}
