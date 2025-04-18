using FluentValidation;
using MovieApp.Application.Common;

namespace MovieApp.Application.Commands.Account.ChangeAvatar;

public class ChangeAvatarCommandValidation : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidation()
    {
        RuleFor(x => x.AvatarType)
            .NotEmpty().WithMessage("Avatar type is required.")
            .IsInEnum().WithMessage("Avatar type is invalid.");
    }
    
}