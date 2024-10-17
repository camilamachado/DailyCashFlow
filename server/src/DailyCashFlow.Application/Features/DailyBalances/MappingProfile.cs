using AutoMapper;
using DailyCashFlow.Application.Features.DailyBalances.Handlers;
using DailyCashFlow.Application.Features.Transactions.Events;
using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.Domain.Features.Transactions;

namespace DailyCashFlow.Application.Features.DailyBalances
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<TransactionCreatedEvent, CalculateDailyBalance.Command>()
				.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
				.ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

			CreateMap<TransactionCreate.Command, Transaction>();
		}
	}
}
