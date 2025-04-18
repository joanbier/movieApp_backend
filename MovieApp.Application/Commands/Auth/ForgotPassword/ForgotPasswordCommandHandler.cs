using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Commands.Auth.ForgotPassword;

internal class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return false;
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetUrl = $"{_configuration["FrontendUrl"]}/auth/reset-password?email={user.Email}&token={token}";
        
        await _emailService.SendEmailAsync(request.Email, "Reset your password", $"This link will be valid for 1 hour. Click here to reset your password: {resetUrl}");

        return true;
    }
}