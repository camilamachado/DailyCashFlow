namespace DailyCashFlow.Infra.ResultPattern
{
	public class Result<TSuccess, TFailure>
	{
		public TSuccess? Success { get; }
		public TFailure? Failure { get; }
		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;

		private Result(TSuccess? success, TFailure? failure, bool isSuccess)
		{
			if (isSuccess && success is null && typeof(TSuccess).IsValueType)
				throw new ArgumentNullException(nameof(success), "Success cannot be null when the result is successful for value types.");

			if (!isSuccess && failure is null)
				throw new ArgumentNullException(nameof(failure), "Failure cannot be null when the result is a failure.");

			Success = success;
			Failure = failure;
			IsSuccess = isSuccess;
		}

		public static Result<TSuccess, TFailure> Ok(TSuccess success)
		{
			return new Result<TSuccess, TFailure>(success, default, true);
		}

		public static Result<TSuccess, TFailure> Fail(TFailure failure)
		{
			return new Result<TSuccess, TFailure>(default, failure, false);
		}

		// Método para converter Result em Task<Result>
		public Task<Result<TSuccess, TFailure>> AsTask()
		{
			return Task.FromResult(this);
		}

		// Conversão implícita de TFailure (falha) para Result<TSuccess, TFailure>
		public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
		{
			return Fail(failure);
		}

		// Conversão implícita de TSuccess (sucesso) para Result<TSuccess, TFailure>
		public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
		{
			return Ok(success);
		}
	}
}
