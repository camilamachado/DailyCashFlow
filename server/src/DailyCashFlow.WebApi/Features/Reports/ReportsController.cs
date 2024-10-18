using AutoMapper;
using DailyCashFlow.Application.Features.Reports.Handlers;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.WebApi.Base;
using DailyCashFlow.WebApi.Features.Reports.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DailyCashFlow.WebApi.Features.Reports
{
	[Route("api/[controller]")]
	public class ReportsController : ApiController
	{
		private readonly IMediator _mediator;

		public ReportsController(IMediator mediator, IMapper mapper) : base(mapper)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Retorna os saldos diários
		/// </summary>
		/// <returns>Retornar a lista de saldos diários.</returns>
		[HttpGet("daily-balance")]
		[EnableQuery]
		public async Task<IActionResult> GetAllDailyBalances()
		{
			var query = new DailyBalancesReport.Query();
			var result = await _mediator.Send(query);

			return CustomListResponse<IQueryable<DailyBalance>, Exception, DailyBalanceReportViewModel>(result);
		}
	}
}
