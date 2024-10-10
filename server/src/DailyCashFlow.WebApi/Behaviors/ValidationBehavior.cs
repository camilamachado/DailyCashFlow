﻿using FluentValidation;
using MediatR;

namespace DailyCashFlow.WebApi.Behaviors
{
	/// <summary>
	/// Comportamento de pipeline responsável por validar as requisições antes de serem processadas pelos handlers.
	/// </summary>
	/// <returns>A resposta do handler se a validação for bem-sucedida, ou lança uma <see cref="ValidationException"/> em caso de falha na validação.</returns>

	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var context = new ValidationContext<TRequest>(request);
			var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

			var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
			if (failures.Count > 0)
			{
				throw new ValidationException(failures);
			}

			return await next();
		}
	}
}
