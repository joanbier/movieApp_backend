using MediatR;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Favorites;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.Application.Commands.Favorites.AddFavorite;

internal class AddFavoriteCommandHandler : ICommandHandler<AddFavoriteCommand>
{
    private readonly IUserFavoriteRepository _userFavoriteRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public AddFavoriteCommandHandler(IUserFavoriteRepository userFavoriteRepository, IMovieRepository movieRepository, 
        IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userFavoriteRepository = userFavoriteRepository;
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    
    
    public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
        {
            throw new MovieNotFoundException(request.MovieId);
        }
        
        var userId = _userContext.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to add movie comments");
        }
        
        var isAlreadyFavorite = await _userFavoriteRepository.IsFavoriteAsync(userId, request.MovieId, cancellationToken);
        if (isAlreadyFavorite)
        {
            throw new MovieIsAlreadyAsFavoriteException(request.MovieId);
        }
        
        var favorite = new UserFavorite
        {
            UserId = userId,
            MovieId = request.MovieId,
            AddedAt = DateTime.UtcNow
        };
        
        _userFavoriteRepository.AddFavorite(favorite);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
}