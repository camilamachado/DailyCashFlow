using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.Domain.Features.Transactions;
using FluentAssertions;
using FluentValidation.TestHelper;
using static DailyCashFlow.Application.Features.Transactions.Handlers.TransactionCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Transactions
{
	[TestFixture]
	public class TransactionCreateValidatorTests
	{
		private Validator _validator;

		[SetUp]
		public void Setup()
		{
			_validator = new Validator();
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_TodosOsDadosValidos_Deve_SerValido()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now.AddSeconds(-1),
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_IdDaCategoriaInvalido_Deve_Falhar()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 0, 
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.CategoryId)
				  .WithErrorMessage("'Category Id' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_DataFutura_Deve_Falhar()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now.AddDays(1),
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			var error = result.ShouldHaveValidationErrorFor(x => x.Date);

			// Verifica se a mensagem de erro contém a parte esperada
			error.Should().Contain(e => e.ErrorMessage.Contains("'Date' deve ser inferior ou igual a"));
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_DataInvalida_Deve_Falhar()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = new DateTime(1800, 1, 1),
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Date)
				  .WithErrorMessage("'Date' deve ser superior ou igual a '01/01/1900 00:00:00'.");
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_ValorInvalido_Deve_Falhar()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 0,
				Description = "Test transaction"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Amount)
				  .WithErrorMessage("'Amount' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarTransacao_Com_DescricaoLonga_Deve_Falhar()
		{
			// Arrange
			var command = new TransactionCreate.Command
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = new string('A', 256) // 256 caracteres
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Description)
				  .WithErrorMessage("'Description' deve ser menor ou igual a 255 caracteres. Você digitou 256 caracteres.");
		}
	}
}
