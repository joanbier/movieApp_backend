using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Actors.GetMoviesByActorId;

public record GetMoviesByActorIdQuery(int Id, int PageNumber, int PageSize) : IRequest<PagedResult<MovieDto>>
{
    
}