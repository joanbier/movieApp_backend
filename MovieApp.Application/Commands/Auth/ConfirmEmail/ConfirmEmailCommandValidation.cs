using FluentValidation;

namespace MovieApp.Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandValidation : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidation()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
        
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}