using DailyCashFlow.Auth.Integration.Tests.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DailyCashFlow.Auth.Integration.Tests.Controllers
{
	[TestFixture]
	public class AuthControllerTests : BaseAuthIntegrationTests
	{
		[Test]
		public async Task GerarToken_Com_CredenciaisValidas_DeveRetornarOk()
		{
			// Arrange
			var validCredentials = new
			{
				Email = "admin@admin.com",
				Password = "C@rref0ur"
			};

			var content = new StringContent(JsonConvert.SerializeObject(validCredentials), Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/auth/token", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var responseContent = await response.Content.ReadAsStringAsync();
			var token = responseContent;
			token.Should().NotBeNullOrWhiteSpace();
		}

		[Test]
		public async Task GerarToken_Com_EmailInexistente_DeveRetornarUnauthorized()
		{
			// Arrange
			var invalidCredentials = new
			{
				Email = "inexistente@exemplo.com",
				Password = "Senha123!"
			};

			var content = new StringContent(JsonConvert.SerializeObject(invalidCredentials), Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/auth/token", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("InvalidCredentialsException");
			problemDetails.Status.Should().Be((int)HttpStatusCode.Unauthorized);
			problemDetails.Detail.Should().Contain("E-mail or password is incorrect.");
		}

		[Test]
		public async Task GerarToken_Com_SenhaIncorreta_DeveRetornarUnauthorized()
		{
			// Arrange
			var invalidCredentials = new
			{
				Email = "admin@admin.com",
				Password = "SenhaErrada!"
			};

			var content = new StringContent(JsonConvert.SerializeObject(invalidCredentials), Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/auth/token", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("InvalidCredentialsException");
			problemDetails.Status.Should().Be((int)HttpStatusCode.Unauthorized);
			problemDetails.Detail.Should().Contain("E-mail or password is incorrect.");
		}

		[Test]
		public async Task GerarToken_Com_DadosInvalidos_DeveRetornarBadRequest()
		{
			// Arrange
			var invalidCredentials = new
			{
				Email = "adminadmin.com", // Email inválido
				Password = "" // Senha vazia
			};

			var content = new StringContent(JsonConvert.SerializeObject(invalidCredentials), Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/auth/token", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var responseContent = await response.Content.ReadAsStringAsync();
			var validationErrors = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			validationErrors.Errors.Should().ContainKey("Email");
			validationErrors.Errors["Email"].Should().Contain("O formato do email é inválido.");

			validationErrors.Errors.Should().ContainKey("Password");
			validationErrors.Errors["Password"].Should().Contain("'Password' deve ser informado.");
		}
	}
}
