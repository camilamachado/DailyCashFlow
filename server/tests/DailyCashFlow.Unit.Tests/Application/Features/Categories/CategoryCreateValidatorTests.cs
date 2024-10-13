using DailyCashFlow.Application.Features.Categories.Handlers;
using FluentValidation.TestHelper;
using static DailyCashFlow.Application.Features.Categories.Handlers.CategoryCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Categories
{
	[TestFixture]
	public class CategoryCreateValidatorTests
	{
		private Validator _validator;

		[SetUp]
		public void Setup()
		{
			_validator = new Validator();
		}

		[Test]
		public void ValidadorDeCriarCategoria_Com_TodosOsDadosValidos_Deve_SerValido()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = "Compras"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void ValidadorDeCriarCategoria_Com_NomeVazio_Deve_Falhar()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = string.Empty,
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Name)
				  .WithErrorMessage("'Name' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarCategoria_Com_NomeLongo_Deve_Falhar()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = new string('A', 51), // 51 caracteres
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Name)
				  .WithErrorMessage("'Name' deve ter entre 1 e 50 caracteres. Você digitou 51 caracteres.");
		}
	}
}
