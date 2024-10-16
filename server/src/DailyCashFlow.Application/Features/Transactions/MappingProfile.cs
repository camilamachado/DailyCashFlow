﻿using AutoMapper;
using DailyCashFlow.Application.Features.DailyBalances.Handlers;
using DailyCashFlow.Application.Features.Transactions.Handlers;
using DailyCashFlow.Domain.Features.Transactions;

namespace DailyCashFlow.Application.Features.Transactions
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CalculateDailyBalance.Command, Transaction>();
		}
	}
}
