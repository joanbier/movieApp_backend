using System.Net;

namespace MovieApp.Domain.Exceptions.Auth;

public class UserNameAlreadyExistsException : MovieAppException
{
    public string UserName { get; set; }

    public UserNameAlreadyExistsException(string userName) : base(
        $"Username '{userName}' is already taken.")
    {
        UserName = userName;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}