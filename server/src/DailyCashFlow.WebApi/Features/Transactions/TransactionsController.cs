using AutoMapper;
using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DailyCashFlow.WebApi.Features.Transactions
{
	[Route("api/[controller]")]
	public class TransactionsController : ApiController
	{
		private readonly IMediator _mediator;

		public TransactionsController(IMediator mediator, IMapper mapper) : base(mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Cria um nova transação no sistema.
		/// </summary>
		/// <param name="command">O comando contendo as informações necessárias para criar uma transação.</param>
		/// <returns>Retorna o identificador da transação criada em caso de sucesso ou uma mensagem de erro caso a criação falhe.</returns>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] TransactionCreate.Command command)
		{
			var result = await _mediator.Send(command);

			return CustomResponse(result);
		}
	}
}
