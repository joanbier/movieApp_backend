using AutoMapper;
using MediatR;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Favorites.GetAllUserFavMovies;

public class GetAllUserFavMoviesQueryHandler : IRequestHandler<GetAllUserFavMoviesQuery, PagedResult<MovieDto>>
{
    private readonly IUserFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetAllUserFavMoviesQueryHandler(IUserFavoriteRepository favoriteRepository, IMapper mapper, IUserContext userContext)
    {
        _favoriteRepository = favoriteRepository;
        _mapper = mapper;
        _userContext = userContext;
    }
    
    public async Task<PagedResult<MovieDto>> Handle(GetAllUserFavMoviesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to add movie comments");
        }
        
        var pagedMovies = await _favoriteRepository.GetAllUserFavAsync(userId, request.PageNumber, request.PageSize, cancellationToken);

        var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(pagedMovies.Items);
        
        foreach (var movieDto in moviesDto)
        {
            movieDto.IsFavorite = true;
        }

        return new PagedResult<MovieDto>(moviesDto, pagedMovies.TotalCount, request.PageNumber, request.PageSize);
    }
}