using AutoMapper;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Configuration.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
    }
}