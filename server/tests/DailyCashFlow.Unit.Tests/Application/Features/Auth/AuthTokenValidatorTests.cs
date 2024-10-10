using DailyCashFlow.Application.Features.Auth.Handlers;
using FluentValidation.TestHelper;

namespace DailyCashFlow.Unit.Tests.Application.Features.Auth
{
	[TestFixture]
	public class AuthTokenValidatorTests
	{
		private AuthToken.Validator _validator;

		[SetUp]
		public void Setup()
		{
			_validator = new AuthToken.Validator();
		}

		[Test]
		public void ValidadorDeObterToken_Com_EmailValidoESenhaValida_Deve_SerValido()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "admin@admin.com",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void ValidadorDeObterToken_Com_EmailVazio_Deve_Falhar()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = string.Empty,
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' deve ser informado.");
		}

		[Test]
		public void ValidadorDeObterToken_Com_EmailFormatoInvalido_Deve_Falhar()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "email_invalido",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("O formato do email é inválido.");
		}

		[Test]
		public void ValidadorDeObterToken_Com_SenhaVazia_Deve_Falhar()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "admin@admin.com",
				Password = string.Empty
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("'Password' deve ser informado.");
		}

		[Test]
		public void ValidadorDeObterToken_Com_SenhaMaiorQue30Caracteres_Deve_Falhar()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "admin@admin.com",
				Password = new string('a', 31)
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("'Password' deve ter entre 0 e 30 caracteres. Você digitou 31 caracteres.");
		}

		[Test]
		public void ValidadorDeObterToken_Com_EmailMaiorQue100Caracteres_Deve_Falhar()
		{
			// Arrange
			var command = new AuthToken.Command
			{
				Email = "adminadminadminadminadminadminadminadminadminadminadminadminadminadminadminadminadminadminadmin@admin.com",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' deve ter entre 1 e 100 caracteres. Você digitou 105 caracteres.");
		}
	}
}