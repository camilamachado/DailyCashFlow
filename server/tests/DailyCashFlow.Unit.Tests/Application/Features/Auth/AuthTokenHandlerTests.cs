using DailyCashFlow.Application.Features.Auth.Handlers;
using DailyCashFlow.Application.Features.Auth.Services;
using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using static DailyCashFlow.Application.Features.Auth.Handlers.AuthToken;

namespace DailyCashFlow.Unit.Tests.Application.Features.Auth
{
	[TestFixture]
	public class AuthTokenHandlerTests
	{
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<IJWTService> _jwtServiceMock;
		private Mock<ILogger<Handler>> _loggerMock;
		private AuthToken.Handler _handler;

		[SetUp]
		public void SetUp()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_jwtServiceMock = new Mock<IJWTService>();
			_loggerMock = new Mock<ILogger<Handler>>();
			_handler = new AuthToken.Handler(_userRepositoryMock.Object, _jwtServiceMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task ObterToken_Com_UsuarioExistenteESenhaValida_Deve_GerarToken()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "admin@admin.com",
				Password = "Senha123!"
			};

			var user = new User
			{
				Id = 1,
				Email = command.Email
			};

			user.SetPassword(command.Password);

			_userRepositoryMock.Setup(repo => repo.GetByEmailNoTrackingAsync(command.Email))
				.ReturnsAsync(user);

			_jwtServiceMock.Setup(jwt => jwt.GenerateJwtToken(It.IsAny<User>()))
				.Returns("jwt_token");

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().Be("jwt_token");
			_jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(user), Times.Once);
		}


		[Test]
		public async Task ObterToken_Com_EmailInexistente_Deve_RetornarInvalidCredentialsException()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "inexistente@exemplo.com",
				Password = "Senha123!"
			};

			_userRepositoryMock.Setup(repo => repo.GetByEmailNoTrackingAsync(command.Email))
				.ReturnsAsync(new NotFoundException("User not found."));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<InvalidCredentialsException>();
			result.Failure.Message.Should().Be("E-mail or password is incorrect.");
			_jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(It.IsAny<User>()), Times.Never);
		}

		[Test]
		public async Task ObterToken_Com_SenhaIncorreta_Deve_RetornarInvalidCredentialsException()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "admin@admin.com",
				Password = "SenhaIncorreta"
			};

			var user = new User
			{
				Id = 1,
				Email = command.Email,
			};

			user.SetPassword(BCrypt.Net.BCrypt.HashPassword("Senha123!"));

			_userRepositoryMock.Setup(repo => repo.GetByEmailNoTrackingAsync(command.Email))
				.ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<InvalidCredentialsException>();
			result.Failure.Message.Should().Be("E-mail or password is incorrect.");
			_jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(It.IsAny<User>()), Times.Never);
		}
	}
}