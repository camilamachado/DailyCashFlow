using AutoMapper;
using DailyCashFlow.Application.Features.Categories.Handlers;
using DailyCashFlow.Domain.Features.Categories;

namespace DailyCashFlow.Application.Features.Categories
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CategoryCreate.Command, Category>();
		}
	}
}
