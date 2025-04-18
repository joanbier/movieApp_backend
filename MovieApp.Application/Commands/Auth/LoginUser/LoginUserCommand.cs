using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Application.Dtos;

namespace MovieApp.Application.Commands.Auth.LoginUser;

public class LoginUserCommand : ICommand<LoginResponseDto>
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}
