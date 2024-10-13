using AutoMapper;
using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using static DailyCashFlow.Application.Features.Categories.Handlers.CategoryCreate;

namespace DailyCashFlow.Unit.Tests.Application.Features.Categorys
{
	[TestFixture]
	public class CategoryCreateHandlerTests
	{
		private Mock<ICategoryFactory> _categoryFactoryMock;
		private Mock<ICategoryRepository> _categoryRepositoryMock;
		private IMapper _mapper;
		private Mock<ILogger<Handler>> _loggerMock;
		private CategoryCreate.Handler _handler;

		[SetUp]
		public void Setup()
		{
			_categoryFactoryMock = new Mock<ICategoryFactory>();
			_categoryRepositoryMock = new Mock<ICategoryRepository>();
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CategoryCreate.Command, Category>();
			});
			_mapper = config.CreateMapper();
			_loggerMock = new Mock<ILogger<Handler>>();
			_handler = new CategoryCreate.Handler(_categoryFactoryMock.Object, _categoryRepositoryMock.Object, _mapper, _loggerMock.Object);
		}

		[Test]
		public async Task CriarCategoria_Com_DadosValidos_Deve_RetornarSucesso()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = "Compras",
			};

			var category = new Category
			{
				Id = 1,
				Name = command.Name,
			};

			_categoryFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>()))
				.ReturnsAsync(category);

			_categoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>()))
				.ReturnsAsync(category);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Success.Should().BeGreaterThanOrEqualTo(1);

			_categoryFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
			_categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
		}

		[Test]
		public async Task CriarCategoria_Com_FalhaAoCriarEntidade_Deve_RetornarFalha()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = "Compras",
			};

			var category = new Category
			{
				Id = 1,
				Name = command.Name,
			};

			_categoryFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>()))
				.ReturnsAsync(Result<Category, Exception>.Fail(new Exception()));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_categoryFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
			_categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
		}

		[Test]
		public async Task CriarCategoria_Com_FalhaAoPersistirDadosNoBanco_Deve_RetornarFalha()
		{
			// Arrange
			var command = new CategoryCreate.Command
			{
				Name = "Usuário Teste",
			};

			var category = new Category
			{
				Id = 1,
				Name = command.Name,
			};

			_categoryFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>()))
				.ReturnsAsync(category);

			_categoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>()))
				.ReturnsAsync(Result<Category, Exception>.Fail(new Exception()));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Failure.Should().BeOfType<Exception>();

			_categoryFactoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
			_categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
		}
	}
}
