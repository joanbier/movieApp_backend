using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Commands.Account.ChangePassword;

internal class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserContext _userContext;

    public ChangePasswordCommandHandler(UserManager<AppUser> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
    }
    
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("You are not authorized to this action");

        var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
        
        if (!result.Succeeded)
        {
            throw new UserCreationFailedException(result.Errors.Select(e => e.Description));
        }
        
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        
        return result.Succeeded;
    }
}