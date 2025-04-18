using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Common;
using MovieApp.Domain.Enums;

namespace MovieApp.Application.Queries.Movies.GetMovies;

public record GetMoviesQuery(
    int PageNumber, 
    int PageSize, 
    MovieSortBy SortBy, 
    bool Descending, 
    string? SearchTerm,
    string? Genre
    ) : IRequest<PagedResult<MovieDto>>
{
    
}