using FluentValidation;

namespace MovieApp.Application.Commands.Auth.RegisterUser;

public class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidation()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MinimumLength(3).WithMessage("UserName min length is 3")
            .MaximumLength(50).WithMessage("UserName max length is 50");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter ('a'-'z')")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter ('A'-'Z')")
            .Matches("[0-9]").WithMessage("Password must have at least one digit ('0'-'9')")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must have at least one non-alphanumeric character");
    }

}