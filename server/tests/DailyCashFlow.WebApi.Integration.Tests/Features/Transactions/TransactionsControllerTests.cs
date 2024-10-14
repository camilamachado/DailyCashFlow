using DailyCashFlow.Common.Tests.Utils;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.WebApi.Integration.Tests.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DailyCashFlow.WebApi.Integration.Tests.Features.Transactions
{
	[TestFixture]
	public class TransactionsControllerTests : BaseWebApiIntegrationTests
	{
		#region POST
		[Test]
		public async Task CriarTransacao_Com_TokenValido_DeveRetornarOk()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newTransaction = new
			{
				CategoryId = 1,
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newTransaction), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/transactions", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var responseContent = await response.Content.ReadAsStringAsync();
			var transactionId = JsonConvert.DeserializeObject<int>(responseContent);

			var transactionCreated = _dbContext.Transactions.AsNoTracking().FirstOrDefault(u => u.Id == transactionId);

			transactionCreated.Should().NotBeNull();
			transactionCreated.CategoryId.Should().Be(newTransaction.CategoryId);
			transactionCreated.Date.Should().Be(newTransaction.Date);
			transactionCreated.Type.Should().Be(newTransaction.Type);
			transactionCreated.Amount.Should().Be(newTransaction.Amount);
			transactionCreated.Description.Should().Be(newTransaction.Description);
		}

		[Test]
		public async Task CriarTransacao_Com_CategoriaInexistente_DeveRetornarNotFound()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newTransaction = new
			{
				CategoryId = 999, // ID de categoria que não existe
				Date = DateTime.Now,
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Test transaction"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newTransaction), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/transactions", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("NotFoundException");
			problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
			problemDetails.Detail.Should().Contain("A category with ID 999 does not exist.");
		}

		[Test]
		public async Task CriarTransacao_Com_DadosInvalidos_DeveRetornarBadRequest()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var newTransaction = new
			{
				CategoryId = 0, // Categoria inválida
				Date = DateTime.Now.AddDays(1), // Data no futuro
				Type = (TransactionType)999, // Tipo inválido
				Amount = -100, // Quantia inválida
				Description = "Test transaction"
			};

			var content = new StringContent(JsonConvert.SerializeObject(newTransaction), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/transactions", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var responseContent = await response.Content.ReadAsStringAsync();
			var validationErrors = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			validationErrors.Errors.Should().ContainKey("CategoryId");
			validationErrors.Errors.Should().ContainKey("Date");
			validationErrors.Errors.Should().ContainKey("Type");
			validationErrors.Errors.Should().ContainKey("Amount");
		}

		[Test]
		public async Task CriarTransacao_Com_TransacaoDuplicada_DeveRetornarConflict()
		{
			// Arrange
			var token = TokenUtils.GerarTokenJWTFalso();

			var existingTransaction = new Transaction()
			{
				CategoryId = 1,
				Date = new DateTime(day: 14, month: 10, year: 2024),
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Existing transaction"
			};

			_dbContext.Transactions.Add(existingTransaction);
			await _dbContext.SaveChangesAsync();

			var duplicateTransaction = new
			{
				CategoryId = 1,
				Date = new DateTime(day: 14, month: 10, year: 2024),
				Type = TransactionType.Credit,
				Amount = 100,
				Description = "Existing transaction"
			};

			var content = new StringContent(JsonConvert.SerializeObject(duplicateTransaction), Encoding.UTF8, "application/json");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Act
			var response = await _client.PostAsync("/api/transactions", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Conflict);

			var responseContent = await response.Content.ReadAsStringAsync();
			var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);

			problemDetails.Should().NotBeNull();
			problemDetails.Title.Should().Be("AlreadyExistsException");
			problemDetails.Status.Should().Be((int)HttpStatusCode.Conflict);
			problemDetails.Detail.Should().Contain("A similar transaction already exists.");
		}
		#endregion
	}
}
