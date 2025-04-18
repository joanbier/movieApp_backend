using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieApp.Application.Common;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Commands.Auth.RegisterUser;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager,  IEmailService emailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var isAlreadyExist = await _userManager.FindByEmailAsync(request.Email);
        if (isAlreadyExist != null)
        {
            throw new UserAlreadyExistsException(request.Email);
        }
        
        var isUserNameAlreadyExist = await _userManager.FindByNameAsync(request.UserName);
        if (isUserNameAlreadyExist != null)
        {
            throw new UserNameAlreadyExistsException(request.UserName);
        }
        
        var user = new AppUser
        {
            UserName = request.UserName, 
            Email = request.Email,
            AvatarUrl = AvatarService.GenerateGravatarUrl(request.Email), 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var isFirstUser = !await _userManager.Users.AnyAsync(cancellationToken);
        var role = isFirstUser ? "Admin" : "User";

        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            throw new UserCreationFailedException(result.Errors.Select(e => e.Description));
        }

        await _userManager.AddToRoleAsync(user, role);
        
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);
        var resetUrl = $"{_configuration["FrontendUrl"]}/auth/confirm-email?userId={user.Id}&token={encodedToken}";
        
        await _emailService.SendEmailAsync(request.Email, "Confirm your email", $"This link will be valid for 1 hour. Click here to confirm your email: {resetUrl}");
        
        
    }
    

}