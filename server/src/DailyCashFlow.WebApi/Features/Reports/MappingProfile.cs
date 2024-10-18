using AutoMapper;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.WebApi.Features.Reports.ViewModels;

namespace DailyCashFlow.WebApi.Features.Reports
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<DailyBalance, DailyBalanceReportViewModel>()
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));
		}
	}
}
