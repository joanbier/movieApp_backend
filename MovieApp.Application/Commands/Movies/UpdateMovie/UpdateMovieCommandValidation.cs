using FluentValidation;

namespace MovieApp.Application.Commands.Movies.UpdateMovie;

public class UpdateMovieCommandValidation : AbstractValidator<UpdateMovieCommand>
{
    private const int FirstMovieYear = 1895;
    
    public UpdateMovieCommandValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title max length is 50");
        
        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required")
            .InclusiveBetween(FirstMovieYear, DateTime.UtcNow.Year)
            .WithMessage($"Year must be between {FirstMovieYear} and {DateTime.UtcNow.Year}");

        RuleFor(x => x.Rating)
            .NotEmpty().WithMessage("Rating is required")
            .InclusiveBetween(1.0f, 10.0f).WithMessage("Rating must be between 1 and 10");
        
        RuleFor(x => x.Director)
            .NotEmpty().WithMessage("Director is required")
            .MaximumLength(50).WithMessage("Director max length is 50");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(5).WithMessage("Description minimum length is 10")
            .MaximumLength(1000).WithMessage("Description max length is 1000");
    }
}