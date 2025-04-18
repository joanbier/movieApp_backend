using FluentValidation;

namespace MovieApp.Application.Commands.Movies;

public class AddMovieCommandValidation : AbstractValidator<AddMovieCommand>
{
    private const int FirstMovieYear = 1895;
    
    public AddMovieCommandValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title max length is 50");
        
        RuleFor(x => x.ReleasedYear)
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
        
        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(100).WithMessage("Description max length is 100");
        
        RuleFor(x => x.PosterLink)
            .MaximumLength(200).WithMessage("Poster link max length is 200");
        
        RuleFor(x => x.Certificate)
            .MaximumLength(10).WithMessage("Certificate max length is 10");
        
        RuleFor(x => x.Runtime)
            .MaximumLength(10).WithMessage("Runtime max length is 10");
    }
}