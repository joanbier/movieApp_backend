using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Commands.Account.ChangeEmail;

internal class ChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserContext _userContext;

    public ChangeEmailCommandHandler(UserManager<AppUser> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
    }
    
    public async Task<bool> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("You are not authorized to this action");
        
        var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordCheck)
            throw new UnauthorizedAccessException("Invalid password.");

        var result = await _userManager.SetEmailAsync(user, request.NewEmail);
        
        if (!result.Succeeded)
        {
            throw new UserCreationFailedException(result.Errors.Select(e => e.Description));
        }
        
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        
        return result.Succeeded;
    }
}