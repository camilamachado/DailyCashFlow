using AutoMapper;
using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.WebApi.Base;
using DailyCashFlow.WebApi.Features.Categories.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DailyCashFlow.WebApi.Features.Categories
{
	[Route("api/[controller]")]
	public class CategoriesController : ApiController
	{
		private readonly IMediator _mediator;

		public CategoriesController(IMediator mediator, IMapper mapper) : base(mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Cria uma nova categoria no sistema.
		/// </summary>
		/// <param name="command">O comando contendo as informações necessárias para criar uma categoria.</param>
		/// <returns>Retorna o identificador da categoria criado em caso de sucesso ou uma mensagem de erro caso a criação falhe.</returns>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CategoryCreate.Command command)
		{
			var result = await _mediator.Send(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// Retorna a coleção de categorias no sistema.
		/// </summary>
		/// <returns>Retorna a coleção de categorias.</returns>
		[HttpGet]
		[EnableQuery]
		public async Task<IActionResult> GetAllAsync()
		{
			var query = new CategoryCollection.Query();
			var result = await _mediator.Send(query);

			return CustomListResponse<IQueryable<Category>, Exception, CategoryViewModel>(result);
		}
	}
}
