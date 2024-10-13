using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Categories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DailyCashFlow.Unit.Tests.Domain.Features.Categories
{
	[TestFixture]
	public class CategoryFactoryTests
	{
		private Mock<ICategoryRepository> _categoryRepositoryMock;
		private Mock<ILogger<CategoryFactory>> _loggerMock;
		private ICategoryFactory _categoryFactory;

		[SetUp]
		public void Setup()
		{
			_categoryRepositoryMock = new Mock<ICategoryRepository>();
			_loggerMock = new Mock<ILogger<CategoryFactory>>();
			_categoryFactory = new CategoryFactory(_categoryRepositoryMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task CriarCategoria_Com_CategoriaNova_Deve_RetornarCategoria()
		{
			// Arrange
			var category = new Category
			{
				Name = "Compras",
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByNameAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			// Act
			var result = await _categoryFactory.CreateAsync(category);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().NotBeNull();

			_categoryRepositoryMock.Verify(x => x.HasAnyByNameAsync(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public async Task CriarCategoria_Com_CategoriaExistente_Deve_RetornarErroJaExistente()
		{
			// Arrange
			var category = new Category
			{
				Name = "Compras",
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByNameAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			// Act
			var result = await _categoryFactory.CreateAsync(category);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<AlreadyExistsException>();
			result.Failure.Message.Should().Contain("A category already exists with the name Compras.");

			_categoryRepositoryMock.Verify(x => x.HasAnyByNameAsync(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public async Task CriarCategoria_Com_FalhaAoVerificarExistencia_Deve_RetornarFalha()
		{
			// Arrange
			var category = new Category
			{
				Name = "Compras",
			};

			_categoryRepositoryMock.Setup(x => x.HasAnyByNameAsync(It.IsAny<string>()))
				.ReturnsAsync((new Exception("Error when checking category")));

			// Act
			var result = await _categoryFactory.CreateAsync(category);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_categoryRepositoryMock.Verify(x => x.HasAnyByNameAsync(It.IsAny<string>()), Times.Once);
		}
	}
}
