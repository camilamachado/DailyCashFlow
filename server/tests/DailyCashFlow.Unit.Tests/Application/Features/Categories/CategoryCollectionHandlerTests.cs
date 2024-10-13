using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DailyCashFlow.Unit.Tests.Application.Features.Categories
{
	[TestFixture]
	public class CategoryCollectionHandlerTests
	{
		private Mock<ICategoryRepository> _categoryRepositoryMock;
		private Mock<ILogger<CategoryCollection.Handler>> _loggerMock;
		private CategoryCollection.Handler _handler;

		[SetUp]
		public void Setup()
		{
			_categoryRepositoryMock = new Mock<ICategoryRepository>();
			_loggerMock = new Mock<ILogger<CategoryCollection.Handler>>();
			_handler = new CategoryCollection.Handler(_categoryRepositoryMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task ObterTodasCategorias_Deve_RetornarCategorias()
		{
			// Arrange
			var categories = new[]
			{
				new Category { Id = 1, Name = "Categoria 1" },
				new Category { Id = 2, Name = "Categoria 2" }
			}.AsQueryable();

			_categoryRepositoryMock.Setup(repo => repo.GetAll())
				.Returns(Result<IQueryable<Category>, Exception>.Ok(categories));

			var query = new CategoryCollection.Query();

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().BeEquivalentTo(categories);

			_categoryRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
		}

		[Test]
		public async Task ObterTodasCategorias_Com_FalhaAoObterDados_Deve_RetornarFalha()
		{
			// Arrange
			var exception = new Exception();

			_categoryRepositoryMock.Setup(repo => repo.GetAll())
				.Returns(Result<IQueryable<Category>, Exception>.Fail(exception));

			var query = new CategoryCollection.Query();

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().Be(exception);

			_categoryRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
		}
	}
}
