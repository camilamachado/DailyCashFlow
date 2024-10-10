using System.Net;

namespace DailyCashFlow.Domain.Exceptions
{
	public abstract class BusinessException : Exception
	{
		public abstract HttpStatusCode StatusCode { get; }

		protected BusinessException(string message) : base(message) { }
	}
}
