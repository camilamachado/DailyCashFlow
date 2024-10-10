using System.Net;

namespace DailyCashFlow.Domain.Exceptions
{
	public class NotFoundException : BusinessException
	{
		public NotFoundException(string message) : base(message) { }

		public override HttpStatusCode StatusCode => HttpStatusCode.NotFound; // 404
	}
}
