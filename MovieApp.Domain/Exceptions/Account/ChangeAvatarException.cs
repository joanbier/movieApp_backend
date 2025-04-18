using System.Net;

namespace MovieApp.Domain.Exceptions.Account;

public class ChangeAvatarException : MovieAppException
{
    public string Email { get; set; }

    public ChangeAvatarException(string email) : base(
        $"Failed to update avatar for user with e-mail {email}.")
    {
        Email = email;
    }
    
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}