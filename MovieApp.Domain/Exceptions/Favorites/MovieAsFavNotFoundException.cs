using System.Net;

namespace MovieApp.Domain.Exceptions.Favorites;

public class MovieAsFavNotFoundException : MovieAppException
{
    public int MovieId { get; set; }

    public MovieAsFavNotFoundException(int movieId) : base(
        $"Movie with id {movieId} not found in favorites.")
    {
        MovieId = movieId;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}