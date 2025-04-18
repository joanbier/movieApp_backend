using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommand : ICommand<bool>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}