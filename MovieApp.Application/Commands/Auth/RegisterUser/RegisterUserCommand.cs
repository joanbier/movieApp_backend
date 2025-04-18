using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Auth.RegisterUser;

public class RegisterUserCommand : ICommand
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [Description(description: "Passwords must be at least 6 characters, at least one non alphanumeric character, at least one digit ('0'-'9'), at least one uppercase ('A'-'Z') and at least one lowercase ('a'-'z').")]
    public string Password { get; set; }
}