using System.Net;

namespace DailyCashFlow.Domain.Exceptions
{
	public class AlreadyExistsException : BusinessException
	{
		public AlreadyExistsException(string message) : base(message) { }

		public override HttpStatusCode StatusCode => HttpStatusCode.Conflict; // 409
	}
}
