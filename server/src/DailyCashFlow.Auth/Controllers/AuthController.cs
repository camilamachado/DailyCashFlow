using DailyCashFlow.Application.Features.Auth.Handlers;
using DailyCashFlow.Auth.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyCashFlow.Auth.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ApiController
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Gera um token de autenticação para um usuário com base nas credenciais fornecidas (email e senha).
		/// </summary>
		/// <param name="command">Comando contendo as credenciais do usuário para autenticação.</param>
		/// <returns>Um token de autenticação JWT, ou um erro se as credenciais forem inválidas.</returns>
		[AllowAnonymous]
		[HttpPost("token")]
		public async Task<IActionResult> Post([FromBody] AuthToken.Command command)
		{
			var result = await _mediator.Send(command);

			return CustomResponse(result);
		}
	}
}
