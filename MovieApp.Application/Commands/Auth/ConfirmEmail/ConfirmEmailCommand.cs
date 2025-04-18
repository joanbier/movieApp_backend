using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommand : ICommand<bool>
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }
}