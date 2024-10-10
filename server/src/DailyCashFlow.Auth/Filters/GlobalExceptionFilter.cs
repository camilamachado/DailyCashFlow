using DailyCashFlow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace DailyCashFlow.Auth.Filters
{
	public class GlobalExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<GlobalExceptionFilter> _logger;

		public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
		{
			_logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			if (context.Exception is ValidationException validationException)
			{
				var errors = validationException.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

				context.Result = new BadRequestObjectResult(new
				{
					Title = "One or more validation errors occurred.",
					Status = 400,
					Errors = errors
				});

				_logger.LogWarning("Validation errors occurred: {Errors}", errors);

				context.ExceptionHandled = true;

				return;
			}

			if (context.Exception is BusinessException businessException)
			{
				var problemDetails = new ValidationProblemDetails
				{
					Title = businessException.GetType().Name,
					Detail = businessException.Message,
					Status = (int)businessException.StatusCode,
				};

				context.Result = new ObjectResult(problemDetails)
				{
					StatusCode = (int)businessException.StatusCode
				};
			}
			else
			{
				var problemDetails = new ValidationProblemDetails
				{
					Title = "An unexpected error occurred.",
					Detail = context.Exception.Message,
					Status = StatusCodes.Status500InternalServerError
				};

				context.Result = new ObjectResult(problemDetails)
				{
					StatusCode = StatusCodes.Status500InternalServerError
				};

				_logger.LogError(context.Exception, "An unexpected error occurred: {Message}", context.Exception.Message);
			}

			context.ExceptionHandled = true;
		}
	}
}
