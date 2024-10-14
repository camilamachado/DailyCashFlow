using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Domain.Features.Transactions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DailyCashFlow.Unit.Tests.Domain.Features.Transactions
{
	[TestFixture]
	public class TransactionFactoryTests
	{
		private Mock<ICategoryRepository> _categoryRepositoryMock;
		private Mock<ITransactionRepository> _transactionRepositoryMock;
		private Mock<ILogger<TransactionFactory>> _loggerMock;
		private ITransactionFactory _transactionFactory;

		[SetUp]
		public void Setup()
		{
			_categoryRepositoryMock = new Mock<ICategoryRepository>();
			_transactionRepositoryMock = new Mock<ITransactionRepository>();
			_loggerMock = new Mock<ILogger<TransactionFactory>>();
			_transactionFactory = new TransactionFactory(_categoryRepositoryMock.Object, _transactionRepositoryMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task CriarTransacao_Com_CategoriaValida_Deve_RetornarTransacao()
		{
			// Arrange
			var transaction = new Transaction
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByIdAsync(transaction.CategoryId))
				.ReturnsAsync(true);

			_transactionRepositoryMock.Setup(x => x.HasDuplicateTransactionAsync(transaction))
				.ReturnsAsync(false);

			// Act
			var result = await _transactionFactory.CreateAsync(transaction);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().NotBeNull();

			_categoryRepositoryMock.Verify(x => x.HasAnyByIdAsync(transaction.CategoryId), Times.Once);
			_transactionRepositoryMock.Verify(x => x.HasDuplicateTransactionAsync(transaction), Times.Once);
		}

		[Test]
		public async Task CriarTransacao_Com_CategoriaInexistente_Deve_RetornarErroCategoriaNaoEncontrada()
		{
			// Arrange
			var transaction = new Transaction
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByIdAsync(transaction.CategoryId))
				.ReturnsAsync(false);

			// Act
			var result = await _transactionFactory.CreateAsync(transaction);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<NotFoundException>();
			result.Failure.Message.Should().Contain("A category with ID 1 does not exist.");

			_categoryRepositoryMock.Verify(x => x.HasAnyByIdAsync(transaction.CategoryId), Times.Once);
			_transactionRepositoryMock.Verify(x => x.HasDuplicateTransactionAsync(transaction), Times.Never);
		}

		[Test]
		public async Task CriarTransacao_Com_TransacaoDuplicada_Deve_RetornarErroTransacaoJaExistente()
		{
			// Arrange
			var transaction = new Transaction
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Debit,
				Amount = 200,
				Description = "Another test transaction"
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByIdAsync(transaction.CategoryId))
				.ReturnsAsync(true);

			_transactionRepositoryMock.Setup(x => x.HasDuplicateTransactionAsync(transaction))
				.ReturnsAsync(true);

			// Act
			var result = await _transactionFactory.CreateAsync(transaction);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<AlreadyExistsException>();
			result.Failure.Message.Should().Contain("A similar transaction already exists.");

			_categoryRepositoryMock.Verify(x => x.HasAnyByIdAsync(transaction.CategoryId), Times.Once);
			_transactionRepositoryMock.Verify(x => x.HasDuplicateTransactionAsync(transaction), Times.Once);
		}

		[Test]
		public async Task CriarTransacao_Com_FalhaAoVerificarCategoria_Deve_RetornarFalha()
		{
			// Arrange
			var transaction = new Transaction
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 300,
				Description = "Test transaction with failure"
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByIdAsync(transaction.CategoryId))
				.ReturnsAsync(new Exception("Error when checking category"));

			// Act
			var result = await _transactionFactory.CreateAsync(transaction);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_categoryRepositoryMock.Verify(x => x.HasAnyByIdAsync(transaction.CategoryId), Times.Once);
			_transactionRepositoryMock.Verify(x => x.HasDuplicateTransactionAsync(transaction), Times.Never);
		}

		[Test]
		public async Task CriarTransacao_Com_FalhaAoVerificarTransacaoDuplicada_Deve_RetornarFalha()
		{
			// Arrange
			var transaction = new Transaction
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 400,
				Description = "Test transaction with duplicate check failure"
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByIdAsync(transaction.CategoryId))
				.ReturnsAsync(true);

			_transactionRepositoryMock.Setup(x => x.HasDuplicateTransactionAsync(transaction))
				.ReturnsAsync(new Exception("Error when checking for duplicate transaction"));

			// Act
			var result = await _transactionFactory.CreateAsync(transaction);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_categoryRepositoryMock.Verify(x => x.HasAnyByIdAsync(transaction.CategoryId), Times.Once);
			_transactionRepositoryMock.Verify(x => x.HasDuplicateTransactionAsync(transaction), Times.Once);
		}
	}
}
