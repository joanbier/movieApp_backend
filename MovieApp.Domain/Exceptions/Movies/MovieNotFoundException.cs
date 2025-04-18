using System.Net;

namespace MovieApp.Domain.Exceptions.Movies;

public class MovieNotFoundException : MovieAppException
{
    public int Id { get; set; }
    public MovieNotFoundException(int id) : base($"Movie with ID {id} was not found.")
        => Id = id;

    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}