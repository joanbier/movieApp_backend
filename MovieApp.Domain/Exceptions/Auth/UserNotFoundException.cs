using System.Net;

namespace MovieApp.Domain.Exceptions.Auth;

public class UserNotFoundException : MovieAppException
{
    public string Id { get; set; }
    public UserNotFoundException(string id) : base($"User with ID {id} was not found.")
        => Id = id;

    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}