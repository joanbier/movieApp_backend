using FluentValidation.TestHelper;
using Moq;
using MovieApp.Application.Commands.Movies;
using MovieApp.Domain.Abstractions;

namespace MovieApp.UnitTests.Commands.Movies.AddMovie;

public class AddMovieCommandValidationTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;

    public AddMovieCommandValidationTest()
    {
        _movieRepositoryMock = new();
    }

    [Fact]
    public void ValidationResult_Should_Not_HaveAnyValidationErrors_WhenAddMovieCommandIsValidated()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = "The Matrix",
            ReleasedYear = 1999,
            Rating = 8.7f,
            Director = "Lana Wachowski",
            Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
            Genre = "Action, Sci-Fi",
            PosterLink =
                "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
            Certificate = "A",
            Runtime = "136 min",
            Gross = 171479930,
            ActorNames = new List<string>() { "Lilly Wachowski", "Keanu Reeves" },
        };
        
        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var validator = new AddMovieCommandValidation();
        
        // Act
        var validationResult = validator.TestValidate(command);
        
        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();

    }

    [Fact]
    public void ValidationResult_Should_HaveValidationErrorForTitle_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = string.Empty,
            ReleasedYear = 1999,
            Rating = 8.7f,
            Director = "Lana Wachowski",
            Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
            Genre = "Action, Sci-Fi",
            PosterLink =
                "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
            Certificate = "A",
            Runtime = "136 min",
            Gross = 171479930,
            ActorNames = new List<string>() { "Lilly Wachowski", "Keanu Reeves" },
        };
        
        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var validator = new AddMovieCommandValidation();
        
        // Act
        var validationResult = validator.TestValidate(command);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void ValidationResult_Should_HaveValidationErrorForTitle_WhenTitleHasMoreThan50Characters()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ReleasedYear = 1999,
            Rating = 8.7f,
            Director = "Lana Wachowski",
            Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
            Genre = "Action, Sci-Fi",
            PosterLink =
                "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
            Certificate = "A",
            Runtime = "136 min",
            Gross = 171479930,
            ActorNames = new List<string>() { "Lilly Wachowski", "Keanu Reeves" },
        };
        
        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var validator = new AddMovieCommandValidation();
        
        // Act
        var validationResult = validator.TestValidate(command);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Fact]
    public void ValidationResult_Should_HaveValidationErrorForRating_WhenRatingIsMoreThan10()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ReleasedYear = 1999,
            Rating = 10.1f,
            Director = "Lana Wachowski",
            Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
            Genre = "Action, Sci-Fi",
            PosterLink =
                "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
            Certificate = "A",
            Runtime = "136 min",
            Gross = 171479930,
            ActorNames = new List<string>() { "Lilly Wachowski", "Keanu Reeves" },
        };
        
        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var validator = new AddMovieCommandValidation();
        
        // Act
        var validationResult = validator.TestValidate(command);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Rating);
    }
    
    [Fact]
    public void ValidationResult_Should_HaveValidationErrorForRating_WhenRatingIsLessThan1()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ReleasedYear = 1999,
            Rating = 0.5f,
            Director = "Lana Wachowski",
            Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
            Genre = "Action, Sci-Fi",
            PosterLink =
                "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
            Certificate = "A",
            Runtime = "136 min",
            Gross = 171479930,
            ActorNames = new List<string>() { "Lilly Wachowski", "Keanu Reeves" },
        };
        
        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var validator = new AddMovieCommandValidation();
        
        // Act
        var validationResult = validator.TestValidate(command);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Rating);
    }
}