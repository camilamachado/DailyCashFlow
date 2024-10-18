using DailyCashFlow.Infra.JsonConverters;
using System.Text.Json.Serialization;

namespace DailyCashFlow.WebApi.Features.Reports.ViewModels
{
	public class DailyBalanceReportViewModel
	{
		[JsonConverter(typeof(CustomDateConverter))]
		public DateTime Date { get; set; }
		public decimal TotalCredit { get; set; }
		public decimal TotalDebit { get; set; }
		public decimal NetBalance { get; set; }
	}
}
