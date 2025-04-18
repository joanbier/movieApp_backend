using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Enums;

namespace MovieApp.Application.Commands.Account.ChangeAvatar;

public class ChangeAvatarCommand : ICommand<bool>
{
    [Required]
    public AvatarType AvatarType { get; set; }
}