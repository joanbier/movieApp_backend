using FluentValidation;

namespace MovieApp.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandValidation : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter ('a'-'z')")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter ('A'-'Z')")
            .Matches("[0-9]").WithMessage("Password must have at least one digit ('0'-'9')")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must have at least one non-alphanumeric character");
    }
}