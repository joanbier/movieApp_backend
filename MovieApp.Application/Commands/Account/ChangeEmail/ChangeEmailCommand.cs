using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Account.ChangeEmail;

public class ChangeEmailCommand : ICommand<bool>
{
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
    
    [Required]
    public string Password { get; set; }
}