using DailyCashFlow.Common.Tests.Utils;
using DailyCashFlow.WebApi.Integration.Tests.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DailyCashFlow.WebApi.Integration.Tests.Features.Users
{
	[TestFixture]
	public class UsersControllerTests : BaseWebApiIntegrationTests
	{
		[Test]
		public async Task CriarUsuario_Com_TokenValido_DeveRetornarOk()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newUser = new
			{
				Name = "Novo Usuário",
				Email = "novo@exemplo.com",
				Password = "Senha123!"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var responseContent = await response.Content.ReadAsStringAsync();
			var userId = JsonConvert.DeserializeObject<int>(responseContent);

			var userCreated = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

			userCreated.Should().NotBeNull();
			userCreated.Name.Should().Be(newUser.Name);
			userCreated.Email.Should().Be(newUser.Email);
		}

		[Test]
		public async Task CriarUsuario_Com_TokenInvalido_DeveRetornarUnauthorized()
		{
			// Arrange
			var invalidToken = "Bearer token_invalido";

			var newUser = new
			{
				Name = "Novo Usuário",
				Email = "novo@exemplo.com",
				Password = "Senha123!"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", invalidToken);

			// Act
			var response = await _client.PostAsync("/api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		}


		[Test]
		public async Task CriarUsuario_Com_EmailDeUsuarioJaExistente_DeveRetornarConflict()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newUser = new
			{
				Name = "Novo Usuário",
				Email = "admin@admin.com",
				Password = "Senha123!"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Conflict);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("AlreadyExistsException");
			problemDetails.Status.Should().Be(409);
			problemDetails.Detail.Should().Contain("A user already exists with the email admin@admin.com.");
		}

		[Test]
		public async Task CriarUsuario_Com_DadosInvalidos_DeveRetornarBadRequest()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newUser = new
			{
				Name = "012345678901234567890123456789012345678901234567890", // Nome muito longo
				Email = "admin.com.br", // Email inválido
				Password = "aaaaaaa" // Senha sem caracteres especiais, números ou maiúsculas
			};

			var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 

			var responseContent = await response.Content.ReadAsStringAsync();
			var validationErrors = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			validationErrors.Errors.Should().ContainKey("Name");
			validationErrors.Errors["Name"].Should().Contain("'Name' deve ter entre 1 e 50 caracteres. Você digitou 51 caracteres.");

			validationErrors.Errors.Should().ContainKey("Email");
			validationErrors.Errors["Email"].Should().Contain("O formato do email é inválido.");

			validationErrors.Errors.Should().ContainKey("Password");
			validationErrors.Errors["Password"].Should().Contain("A senha deve conter pelo menos uma letra maiúscula.");
			validationErrors.Errors["Password"].Should().Contain("A senha deve conter pelo menos um número.");
			validationErrors.Errors["Password"].Should().Contain("A senha deve conter pelo menos um caractere especial (!? *.).");
		}
	}
}
