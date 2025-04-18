using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Actors.GetActors;

public record GetActorsQuery(int PageNumber, int PageSize) : IRequest<PagedResult<ActorDto>>
{
    
}