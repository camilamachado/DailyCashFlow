using DailyCashFlow.Infra.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyCashFlow.WebApi.Base
{
	[ApiController]
	[Authorize]
	public abstract class ApiController : ControllerBase
	{
		/// <summary>
		/// Retorna uma resposta customizada baseada no padrão Result.
		/// </summary>
		/// <typeparam name="TFailure">Tipo do valor de falha, que deve ser uma Exception</typeparam>
		/// <typeparam name="TSuccess">Tipo do valor de sucesso</typeparam>
		/// <param name="result">Resultado da operação</param>
		/// <returns>ActionResult com o resultado</returns>
		protected ActionResult CustomResponse<TSuccess, TFailure>(Result<TSuccess, TFailure> result)
		{
			if (result.IsSuccess && ModelState.IsValid)
			{
				return Ok(result.Success);
			}

			if (result.IsFailure)
			{
				throw result.Failure as Exception;
			}

			// Se não for sucesso nem falha, lançar uma exceção inesperada
			throw new InvalidOperationException("Contact your system administrators.");
		}
	}
}
