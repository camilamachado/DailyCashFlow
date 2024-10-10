using AutoMapper;
using DailyCashFlow.Application.Features.Users.Handlers;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.ResultPattern;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using static DailyCashFlow.Application.Features.Users.Handlers.UserCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Users
{
	[TestFixture]
	public class UserCreateHandlerTests
	{
		private Mock<IUserFactory> _userFactoryMock;
		private Mock<IUserRepository> _userRepositoryMock;
		private IMapper _mapper;
		private Mock<ILogger<Handler>> _loggerMock;
		private UserCreate.Handler _handler;

		[SetUp]
		public void Setup()
		{
			_userFactoryMock = new Mock<IUserFactory>();
			_userRepositoryMock = new Mock<IUserRepository>();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<UserCreate.Command, User>();
			});
			_mapper = config.CreateMapper();
			_loggerMock = new Mock<ILogger<Handler>>();
			_handler = new UserCreate.Handler(_userFactoryMock.Object, _userRepositoryMock.Object, _mapper, _loggerMock.Object);
		}

		[Test]
		public async Task CriarUsuario_Com_DadosValidos_Deve_RetornarSucesso()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Senha123"
			};

			var user = new User
			{
				Id = 1,
				Name = command.Name,
				Email = command.Email
			};

			user.SetPassword(command.Password);

			_userFactoryMock.Setup(x => x.CreateAsync(It.IsAny<User>()))
				.ReturnsAsync(user);

			_userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
				.ReturnsAsync(user);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().BeGreaterThanOrEqualTo(1);

			_userFactoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
			_userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
		}

		[Test]
		public async Task CriarUsuario_Com_FalhaAoCriarEntidade_Deve_RetornarFalha()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Senha123"
			};

			var user = new User
			{
				Id = 1,
				Name = command.Name,
				Email = command.Email
			};

			user.SetPassword(command.Password);

			_userFactoryMock.Setup(x => x.CreateAsync(It.IsAny<User>()))
				.ReturnsAsync(Result<User, Exception>.Fail(new Exception()));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_userFactoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
			_userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
		}

		[Test]
		public async Task CriarUsuario_Com_FalhaAoPersistirDadosNoBanco_Deve_RetornarFalha()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Senha123"
			};

			var user = new User
			{
				Id = 1,
				Name = command.Name,
				Email = command.Email
			};

			user.SetPassword(command.Password);

			_userFactoryMock.Setup(x => x.CreateAsync(It.IsAny<User>()))
				.ReturnsAsync(user);

			_userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
				.ReturnsAsync(Result<User, Exception>.Fail(new Exception()));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_userFactoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
			_userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
		}
	}
}
