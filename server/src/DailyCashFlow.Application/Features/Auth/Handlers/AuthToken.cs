using DailyCashFlow.Application.Features.Auth.Services;
using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Auth.Handlers
{
	public class AuthToken
	{
		public class Command : IRequest<Result<string, Exception>>
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(a => a.Email)
					.NotEmpty()
					.Length(1, 100)
					.EmailAddress().WithMessage("O formato do email é inválido.");

				RuleFor(a => a.Password)
					.NotEmpty()
					.Length(0, 30);
			}
		}

		public class Handler : IRequestHandler<Command, Result<string, Exception>>
		{
			private readonly IUserRepository _userRepository;
			private readonly IJWTService _jwtService;
			private readonly ILogger<Handler> _logger;

			public Handler(IUserRepository userRepository, IJWTService jwtService, ILogger<Handler> logger)
			{
				_userRepository = userRepository;
				_jwtService = jwtService;
				_logger = logger;
			}

			public async Task<Result<string, Exception>> Handle(Command request, CancellationToken cancellationToken)
			{
				var getUserCallback = await _userRepository.GetByEmailNoTrackingAsync(request.Email);
				if (getUserCallback.IsFailure)
				{
					if (getUserCallback.Failure is NotFoundException)
					{
						_logger.LogWarning("Authentication attempt failed for email {Email}: User not found.", request.Email);

						return new InvalidCredentialsException("E-mail or password is incorrect.");
					}

					_logger.LogError(getUserCallback.Failure, "Failed to retrieve user for email {Email}.", request.Email);
					return getUserCallback.Failure;
				}

				var user = getUserCallback.Success;

				// Verifica se a senha fornecida corresponde à senha criptografada armazenada
				if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
				{
					_logger.LogWarning("Authentication attempt failed for email {Email}: Incorrect password.", request.Email);

					return new InvalidCredentialsException("E-mail or password is incorrect.");
				}

				var token = _jwtService.GenerateJwtToken(user);

				return token;
			}
		}
	}
}
