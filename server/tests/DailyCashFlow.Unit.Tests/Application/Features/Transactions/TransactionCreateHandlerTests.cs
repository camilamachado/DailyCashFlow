using AutoMapper;
using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.Domain.Features.Transactions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using static DailyCashFlow.Application.Features.Transactions.Handlers.TransactionCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Transactions
{
	[TestFixture]
	public class TransactionCreateHandlerTests
	{
		private Mock<ITransactionFactory> _transactionFactoryMock;
		private Mock<ITransactionRepository> _transactionRepositoryMock;
		private IMapper _mapper;
		private Mock<ILogger<Handler>> _loggerMock;
		private TransactionCreate.Handler _handler;

		[SetUp]
		public void Setup()
		{
			_transactionFactoryMock = new Mock<ITransactionFactory>();
			_transactionRepositoryMock = new Mock<ITransactionRepository>();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<TransactionCreate.Command, Transaction>();
			});
			_mapper = config.CreateMapper();
			_loggerMock = new Mock<ILogger<Handler>>();
			_handler = new TransactionCreate.Handler(_transactionFactoryMock.Object, _transactionRepositoryMock.Object, _mapper, _loggerMock.Object);
		}

		[Test]
		public async Task CriarTransacao_Com_DadosValidos_Deve_RetornarSucesso()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			var transaction = new Transaction
			{
				Id = 1,
				CategoryId = command.CategoryId,
				Date = command.Date,
				Type = command.Type,
				Amount = command.Amount,
				Description = command.Description
			};

			_transactionFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Transaction>()))
				.ReturnsAsync(transaction);

			_transactionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
				.ReturnsAsync(transaction);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().BeGreaterThanOrEqualTo(1);

			_transactionFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
			_transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
		}

		[Test]
		public async Task CriarTransacao_Com_FalhaAoCriarEntidade_Deve_RetornarFalha()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			var transaction = new Transaction
			{
				CategoryId = command.CategoryId,
				Date = command.Date,
				Type = command.Type,
				Amount = command.Amount,
				Description = command.Description
			};

			_transactionFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Transaction>()))
				.ReturnsAsync(new Exception("Failed to create transaction."));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_transactionFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
			_transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Never);
		}

		[Test]
		public async Task CriarTransacao_Com_FalhaAoPersistirDadosNoBanco_Deve_RetornarFalha()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			var transaction = new Transaction
			{
				Id = 1,
				CategoryId = command.CategoryId,
				Date = command.Date,
				Type = command.Type,
				Amount = command.Amount,
				Description = command.Description
			};

			_transactionFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Transaction>()))
				.ReturnsAsync(transaction);

			_transactionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
				.ReturnsAsync(new Exception("Failed to add transaction."));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_transactionFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
			_transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
		}
	}
}
