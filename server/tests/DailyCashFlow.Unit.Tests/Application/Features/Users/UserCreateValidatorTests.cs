using DailyCashFlow.Application.Features.Users.Handlers;
using FluentValidation.TestHelper;
using static DailyCashFlow.Application.Features.Users.Handlers.UserCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Users
{
	[TestFixture]
	public class UserCreateValidatorTests
	{
		private Validator _validator;

		[SetUp]
		public void Setup()
		{
			_validator = new Validator();
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_TodosOsDadosValidos_Deve_SerValido()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_NomeVazio_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = string.Empty,
				Email = "teste@exemplo.com",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Name)
				  .WithErrorMessage("'Name' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_NomeLongo_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = new string('A', 51), // 51 caracteres
				Email = "teste@exemplo.com",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Name)
				  .WithErrorMessage("'Name' deve ter entre 1 e 50 caracteres. Você digitou 51 caracteres.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_EmailVazio_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = string.Empty,
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				  .WithErrorMessage("'Email' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_EmailInvalido_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "emailinvalido",
				Password = "Senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				  .WithErrorMessage("O formato do email é inválido.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaVazia_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = string.Empty
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("'Password' deve ser informado.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaCurtas_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Curta1!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("'Password' deve ter entre 8 e 12 caracteres. Você digitou 7 caracteres.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaSemMaiusculas_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "senha123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("A senha deve conter pelo menos uma letra maiúscula.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaSemMinusculas_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "SENHA123!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("A senha deve conter pelo menos uma letra minúscula.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaSemNumeros_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "SenhaSemNumero!"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("A senha deve conter pelo menos um número.");
		}

		[Test]
		public void ValidadorDeCriarUsuario_Com_SenhaSemCaractereEspecial_Deve_Falhar()
		{
			// Arrange
			var command = new UserCreate.Command
			{
				Name = "Usuário Teste",
				Email = "teste@exemplo.com",
				Password = "Senha123"
			};

			// Act
			var result = _validator.TestValidate(command);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				  .WithErrorMessage("A senha deve conter pelo menos um caractere especial (!? *.).");
		}
	}
}
