using System.Net;

namespace MovieApp.Domain.Exceptions.Auth;

public class UserAlreadyExistsException : MovieAppException
{
    public string Email { get; set; }

    public UserAlreadyExistsException(string email) : base(
        $"User with e-mail {email} already exists.")
    {
        Email = email;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}