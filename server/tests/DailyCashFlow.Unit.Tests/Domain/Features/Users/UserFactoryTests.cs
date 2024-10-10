using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DailyCashFlow.Unit.Tests.Domain.Features.Users
{
	[TestFixture]
	public class UserFactoryTests
	{
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<ILogger<UserFactory>> _loggerMock;
		private IUserFactory _userFactory;

		[SetUp]
		public void Setup()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_loggerMock = new Mock<ILogger<UserFactory>>();
			_userFactory = new UserFactory(_userRepositoryMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task CriarUsuario_Com_UsuarioNovo_Deve_CriptografarSenhaERetornarUsuario()
		{
			// Arrange
			var user = new User
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com"
			};

			user.SetPassword("Senha123");

			_userRepositoryMock.Setup(x => x.HasAnyByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			// Act
			var result = await _userFactory.CreateAsync(user);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().NotBeNull();
			result.Success.Password.Should().NotBe("Senha123");

			_userRepositoryMock.Verify(x => x.HasAnyByEmailAsync(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public async Task CriarUsuario_Com_UsuarioExistente_Deve_RetornarErroJaExistente()
		{
			// Arrange
			var user = new User
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com"
			};

			_userRepositoryMock.Setup(x => x.HasAnyByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			// Act
			var result = await _userFactory.CreateAsync(user);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<AlreadyExistsException>();
			result.Failure.Message.Should().Contain("A user already exists with the email");

			_userRepositoryMock.Verify(x => x.HasAnyByEmailAsync(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public async Task CriarUsuario_Com_FalhaAoVerificarExistencia_Deve_RetornarFalha()
		{
			// Arrange
			var user = new User
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com"
			};

			_userRepositoryMock.Setup(x => x.HasAnyByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((new Exception("Erro ao verificar usuário")));

			// Act
			var result = await _userFactory.CreateAsync(user);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_userRepositoryMock.Verify(x => x.HasAnyByEmailAsync(It.IsAny<string>()), Times.Once);
		}
	}
}

