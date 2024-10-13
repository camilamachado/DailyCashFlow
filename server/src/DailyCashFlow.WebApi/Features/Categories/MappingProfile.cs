using AutoMapper;
using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.WebApi.Features.Categories.ViewModels;

namespace DailyCashFlow.WebApi.Features.Categories
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Category, CategoryViewModel>();
		}
	}
}
