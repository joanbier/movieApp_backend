using System.Net;

namespace MovieApp.Domain.Exceptions.Auth;

public class UserCreationFailedException : MovieAppException
{
    public UserCreationFailedException(IEnumerable<string> errors) 
        : base($"User creation failed: {string.Join(", ", errors)}")
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}