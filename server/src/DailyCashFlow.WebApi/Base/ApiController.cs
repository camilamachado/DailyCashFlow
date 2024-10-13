using AutoMapper;
using AutoMapper.QueryableExtensions;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyCashFlow.WebApi.Base
{
	[ApiController]
	[Authorize]
	public abstract class ApiController : ControllerBase
	{
		private readonly IMapper _mapper;

		protected ApiController(IMapper mapper)
		{
			_mapper = mapper;
		}

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

		/// <summary>
		/// Retorna uma resposta customizada baseada no padrão Result, mapeando uma coleção para uma View Model.
		/// </summary>
		/// <typeparam name="TSuccess">Tipo do valor de sucesso, que deve ser um IQueryable de qualquer tipo.</typeparam>
		/// <typeparam name="TFailure">Tipo do valor de falha, que deve ser uma Exception.</typeparam>
		/// <typeparam name="TViewModel">Tipo da View Model para o qual os dados serão projetados.</typeparam>
		/// <param name="result">Resultado da operação.</param>
		/// <returns>ActionResult com a lista projetada de View Models.</returns>
		protected ActionResult CustomListResponse<TSuccess, TFailure, TViewModel>(Result<TSuccess, TFailure> result)
			where TSuccess : IQueryable
		{
			if (result.IsSuccess && ModelState.IsValid)
			{
				var projectedResult = result.Success.ProjectTo<TViewModel>(_mapper.ConfigurationProvider);
				return Ok(projectedResult);
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
