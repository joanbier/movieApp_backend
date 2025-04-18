using System.Net;
using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Commands.Auth.ConfirmEmail;

internal class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    
    public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UserNotFoundException(request.UserId);
        
        if (user.EmailConfirmed)
        {
            throw new UserEmailConfirmedException(user.Email);
        }
        
        var decodedToken = WebUtility.UrlDecode(request.Token);
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        user.UpdatedAt = DateTime.UtcNow;
        
        return result.Succeeded;
    }
}