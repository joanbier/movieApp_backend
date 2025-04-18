using System.Net;

namespace MovieApp.Domain.Exceptions.Auth;

public class UserEmailConfirmedException : MovieAppException
{
    public string Email { get; set; }

    public UserEmailConfirmedException(string email) : base(
        $"Email {email} has been already confirmed.")
    {
        Email = email;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}