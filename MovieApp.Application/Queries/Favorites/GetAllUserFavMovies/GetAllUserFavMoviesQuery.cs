using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Favorites.GetAllUserFavMovies;

public record GetAllUserFavMoviesQuery(int PageNumber, int PageSize) : IRequest<PagedResult<MovieDto>>
{}