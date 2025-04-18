using FluentValidation;

namespace MovieApp.Application.Commands.Comments.AddComment;

public class AddCommentCommandValidation : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidation()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(1000).WithMessage("Content max length is 1000");

        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("Movie Id is required")
            .NotNull().WithMessage("Movie Id is required");
    }
}