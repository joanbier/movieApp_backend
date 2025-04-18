using MediatR;
using MovieApp.Application.Dtos;

namespace MovieApp.Application.Queries.Movies.GetMovieById;

public record GetMovieByIdQuery(int Id) : IRequest<MovieDetailsDto>
{
    
}