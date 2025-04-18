using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;

namespace MovieApp.Application.Commands.Auth.LoginUser;

internal class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            throw new UnauthorizedAccessException("E-mail has not been confirmed.");
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);
        var result = new LoginResponseDto() { Token = token };

        return result;
    }
    
}