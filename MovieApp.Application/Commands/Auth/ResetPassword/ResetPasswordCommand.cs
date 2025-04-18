using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommand : ICommand<bool>
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public string NewPassword { get; set; }
}