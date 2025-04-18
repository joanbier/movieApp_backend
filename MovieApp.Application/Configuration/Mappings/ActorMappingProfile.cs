using AutoMapper;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Configuration.Mappings;

public class ActorMappingProfile : Profile
{
    public ActorMappingProfile()
    {
        CreateMap<Actor, ActorDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}