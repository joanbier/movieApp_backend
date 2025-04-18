using AutoMapper;
using MediatR;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;

namespace MovieApp.Application.Queries.Movies.GetMovieById;

internal class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDetailsDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUserFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IUserFavoriteRepository favoriteRepository, IMapper mapper , IUserContext userContext)
    {
        _movieRepository = movieRepository;
        _favoriteRepository = favoriteRepository;
        _mapper = mapper;
        _userContext = userContext;
    }
    
    public async Task<MovieDetailsDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id, cancellationToken);
        
        var movieDto = _mapper.Map<MovieDetailsDto>(movie);
        
        var userId = _userContext.UserId;
        // If user is logged in
        if (!string.IsNullOrEmpty(userId))
        {
            var userFavoriteMovieIds = await _favoriteRepository.GetUserFavoriteMovieIdsAsync(userId, cancellationToken);
            movieDto.IsFavorite = userFavoriteMovieIds.Contains(movieDto.Id);
        }
        
        return movieDto;
    }
}