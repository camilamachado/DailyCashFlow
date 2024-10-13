using DailyCashFlow.Common.Tests.Utils;
using DailyCashFlow.WebApi.Features.Categories.ViewModels;
using DailyCashFlow.WebApi.Integration.Tests.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DailyCashFlow.WebApi.Integration.Tests.Features.Categories
{
	[TestFixture]
	public class CategoriesControllerTests : BaseWebApiIntegrationTests
	{
		#region POST
		[Test]
		public async Task CriarCategoria_Com_TokenValido_DeveRetornarOk()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newCategory = new
			{
				Name = "New Category",
			};

			var content = new StringContent(JsonConvert.SerializeObject(newCategory), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/categories", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var responseContent = await response.Content.ReadAsStringAsync();
			var categoryId = JsonConvert.DeserializeObject<int>(responseContent);

			var categoryCreated = _dbContext.Categories.FirstOrDefault(u => u.Id == categoryId);

			categoryCreated.Should().NotBeNull();
			categoryCreated.Name.Should().Be(newCategory.Name);
		}

		[Test]
		public async Task CriarCategoria_Com_NomeDeCategoriaJaExistente_DeveRetornarConflict()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newCategory = new
			{
				Name = "Compras",
			};

			var content = new StringContent(JsonConvert.SerializeObject(newCategory), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/categories", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Conflict);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("AlreadyExistsException");
			problemDetails.Status.Should().Be(409);
			problemDetails.Detail.Should().Contain("A category already exists with the name Compras.");
		}

		[Test]
		public async Task CriarCategoria_Com_DadosInvalidos_DeveRetornarBadRequest()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newCategory = new
			{
				Name = "012345678901234567890123456789012345678901234567890", // Nome muito longo
			};

			var content = new StringContent(JsonConvert.SerializeObject(newCategory), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/categories", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var responseContent = await response.Content.ReadAsStringAsync();
			var validationErrors = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			validationErrors.Errors.Should().ContainKey("Name");
			validationErrors.Errors["Name"].Should().Contain("'Name' deve ter entre 1 e 50 caracteres. Você digitou 51 caracteres.");
		}
		#endregion

		#region GET
		[Test]
		public async Task ObterTodasCategorias_Com_TokenValido_DeveRetornarTodasAsCategorias()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.GetAsync("/api/categories");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var responseContent = await response.Content.ReadAsStringAsync();
			var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(responseContent);

			categories.Should().NotBeNull();
			categories.Should().HaveCount(7);
			categories.Select(c => c.Name).Should()
				.Contain(new[]
				{
					"Despesas Operacionais",
					"Compras",
					"Pagamentos",
					"Perdas",
					"Vendas",
					"Recebimentos",
					"Outras Receitas"
				});
		}
		#endregion
	}
}
