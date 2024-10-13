using AutoMapper;
using DailyCashFlow.Application.Features.Users.Handlers;
using DailyCashFlow.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DailyCashFlow.WebApi.Features.Users
{
	[Route("api/[controller]")]
	public class UsersController : ApiController
	{
		private readonly IMediator _mediator;

		public UsersController(IMediator mediator, IMapper mapper) : base(mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Cria um novo usuário no sistema.
		/// </summary>
		/// <param name="command">O comando contendo as informações necessárias para criar um usuário.</param>
		/// <returns>Retorna o identificador do usuário criado em caso de sucesso ou uma mensagem de erro caso a criação falhe.</returns>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] UserCreate.Command command)
		{
			var result = await _mediator.Send(command);

			return CustomResponse(result);
		}
	}
}
