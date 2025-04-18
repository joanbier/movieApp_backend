using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Account.ChangePassword;

public class ChangePasswordCommand : ICommand<bool>
{
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string NewPassword { get; set; }
}