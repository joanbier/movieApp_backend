using AutoMapper;
using MediatR;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Movies.GetMovies;

internal class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, PagedResult<MovieDto>>
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository;
    private readonly IUserFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetMoviesQueryHandler(IMovieReadOnlyRepository movieReadOnlyRepository, IUserFavoriteRepository favoriteRepository, IMapper mapper, IUserContext userContext)
    {
        _movieReadOnlyRepository = movieReadOnlyRepository;
        _favoriteRepository = favoriteRepository;
        _mapper = mapper;
        _userContext = userContext;
    }
    
    public async Task<PagedResult<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var pagedMovies = await _movieReadOnlyRepository.GetAllAsync(
            request.PageNumber, request.PageSize, request.SortBy, request.Descending, request.SearchTerm, request.Genre, cancellationToken);
        var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(pagedMovies.Items);
        
        var userId = _userContext.UserId;
        // If user is logged in
        if (!string.IsNullOrEmpty(userId))
        {
            var userFavoriteMovieIds = await _favoriteRepository.GetUserFavoriteMovieIdsAsync(userId, cancellationToken);
            foreach (var movieDto in moviesDto)
            {
                movieDto.IsFavorite = userFavoriteMovieIds.Contains(movieDto.Id);
            }
        }
        return new PagedResult<MovieDto>(moviesDto, pagedMovies.TotalCount, request.PageNumber, request.PageSize);
        
    }
}