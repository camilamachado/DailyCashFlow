using AutoMapper;
using DailyCashFlow.Application.Features.Users.Handlers;
using DailyCashFlow.Domain.Features.Users;

namespace DailyCashFlow.Application.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreate.Command, User>();
        }
    }
}
