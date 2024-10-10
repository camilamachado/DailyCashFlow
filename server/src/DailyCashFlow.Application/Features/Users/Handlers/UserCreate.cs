using AutoMapper;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Application.Features.Users.Handlers
{
	public class UserCreate
	{
		public class Command : IRequest<Result<int, Exception>>
		{
			public string Name { get; set; }
			public string Email { get; set; }
			public string Password { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(a => a.Name).NotEmpty().Length(1, 50);

				RuleFor(a => a.Email)
					.NotEmpty()
					.Length(1, 100)
					.EmailAddress().WithMessage("O formato do email é inválido.");

				RuleFor(a => a.Password)
					.NotEmpty()
					.Length(8, 12)
					.Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
					.Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
					.Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
					.Matches(@"[\!\?\*\.]").WithMessage("A senha deve conter pelo menos um caractere especial (!? *.).");
			}
		}

		public class Handler : IRequestHandler<Command, Result<int, Exception>>
		{
			private readonly IUserFactory _userFactory;
			private readonly IUserRepository _userRepository;
			private readonly IMapper _mapper;
			private readonly ILogger<Handler> _logger;

			public Handler(IUserFactory userFactory, IUserRepository userRepository, IMapper mapper, ILogger<Handler> logger)
			{
				_userFactory = userFactory;
				_userRepository = userRepository;
				_mapper = mapper;
				_logger = logger;
			}

			public async Task<Result<int, Exception>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = _mapper.Map<User>(request);

				var createUserCallback = await _userFactory.CreateAsync(user);
				if (createUserCallback.IsFailure)
				{
					_logger.LogError(createUserCallback.Failure, "Failed to create user with email {Email}", user.Email);
					return createUserCallback.Failure;
				}

				var addUserCallback = await _userRepository.AddAsync(createUserCallback.Success);
				if (addUserCallback.IsFailure)
				{
					_logger.LogError(addUserCallback.Failure, "Failed to add user to repository with email {Email}", user.Email);
					return addUserCallback.Failure;
				}

				return addUserCallback.Success.Id;
			}
		}
	}
}
