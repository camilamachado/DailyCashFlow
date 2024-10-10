using System.Net;

namespace DailyCashFlow.Domain.Exceptions
{
	public class InvalidCredentialsException : BusinessException
	{
		public InvalidCredentialsException(string message) : base(message) { }

		public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized; // 401
	}
}