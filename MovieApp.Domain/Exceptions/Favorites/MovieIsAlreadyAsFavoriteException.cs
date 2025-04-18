using System.Net;

namespace MovieApp.Domain.Exceptions.Favorites;

public class MovieIsAlreadyAsFavoriteException : MovieAppException
{
    public int MovieId { get; set; }

    public MovieIsAlreadyAsFavoriteException(int movieId) : base(
        $"Movie with id {movieId} is already in favorites.")
    {
        MovieId = movieId;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}