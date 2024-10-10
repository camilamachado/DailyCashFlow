using System.Net;

namespace DailyCashFlow.Domain.Exceptions
{
	public class ForbiddenException : BusinessException
	{
		public ForbiddenException(string message) : base(message) { }

		public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden; // 403
	}
}
