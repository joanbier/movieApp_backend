using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Common;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Account;

namespace MovieApp.Application.Commands.Account.ChangeAvatar;

internal class ChangeAvatarCommandHandler : ICommandHandler<ChangeAvatarCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserContext _userContext;
    
    public ChangeAvatarCommandHandler(UserManager<AppUser> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
    }

    public async Task<bool> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("You are not authorized to this action");
        
        
        var newAvatar = AvatarService.ChangeAvatarType(user.Email, request.AvatarType);
        
        user.AvatarUrl = newAvatar;
        user.UpdatedAt = DateTime.UtcNow;
        
        var result = await _userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            throw new ChangeAvatarException(user.Email);
        }
        
        return result.Succeeded;
    }
}