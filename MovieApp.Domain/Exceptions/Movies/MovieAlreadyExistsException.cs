using System.Net;

namespace MovieApp.Domain.Exceptions.Movies;

public class MovieAlreadyExistsException : MovieAppException
{
    public string Title { get; set; }
    public string Director { get; set; }

    public MovieAlreadyExistsException(string title, string director) : base(
        $"Movie with title {title} directed by {director} already exists.")
    {
        Title = title;
        Director = director;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}