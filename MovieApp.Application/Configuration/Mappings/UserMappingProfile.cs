using AutoMapper;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Configuration.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<AppUser, UserDto>();
    }
}
