using AutoMapper;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Configuration.Mappings;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<Movie, MovieDetailsDto>()
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.MovieActors.Select(ma => ma.Actor).ToList()))
            .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.FavoritedBy.Count));
    }
}