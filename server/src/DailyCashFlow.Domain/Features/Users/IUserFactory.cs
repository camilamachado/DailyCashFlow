using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.Extensions.Logging;

namespace DailyCashFlow.Domain.Features.Users
{
	public interface IUserFactory
	{
		/// <summary>
		/// Cria um novo usuário aplicando regras de negócio, como verificação de existência e criptografia de senha.
		/// </summary>
		Task<Result<User, Exception>> CreateAsync(User user);
	}

	public class UserFactory : IUserFactory
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserFactory> _logger;

		public UserFactory(IUserRepository userRepository, ILogger<UserFactory> logger)
		{
			_userRepository = userRepository;
			_logger = logger;
		}

		public async Task<Result<User, Exception>> CreateAsync(User user)
		{
			var hasAnyUserCallback = await _userRepository.HasAnyByEmailAsync(user.Email);
			if (hasAnyUserCallback.IsFailure)
			{
				_logger.LogError(hasAnyUserCallback.Failure, "Error checking if user {Email} exists", user.Email);

				return hasAnyUserCallback.Failure;
			}
			else if (hasAnyUserCallback.Success)
			{
				return new AlreadyExistsException($"A user already exists with the email {user.Email}.");
			}

			user.SetPassword(user.Password);

			return user;
		}
	}

}
