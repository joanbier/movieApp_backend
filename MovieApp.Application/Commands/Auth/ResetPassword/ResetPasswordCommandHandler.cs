using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Commands.Auth.ResetPassword;

internal class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return false;

        user.UpdatedAt = DateTime.UtcNow;
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        return result.Succeeded;
    }
}