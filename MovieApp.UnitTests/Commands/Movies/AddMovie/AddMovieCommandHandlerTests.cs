using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MovieApp.Application.Commands.Movies;
using MovieApp.Application.Configuration.Mappings;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.UnitTests.Commands.Movies.AddMovie;

public class AddMovieCommandHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IActorRepository> _actorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ILogger<AddMovieCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddMovieCommandHandlerTests()
    {
        _movieRepositoryMock = new ();
        _actorRepositoryMock = new();
        _unitOfWorkMock = new();
        _mapper = MapperHelper.CreateMapper(new MovieMappingProfile());
        _logger = new Logger<AddMovieCommandHandler>(new LoggerFactory());
    }

    [Fact]
    public async Task Handle_Should_CallAddOnReposiotry_WhenTitleAndDirectorIsUnique()
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

        _movieRepositoryMock.Setup(x => x.Add(It.IsAny<Movie>()));

        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var handler = new AddMovieCommandHandler(
            _movieRepositoryMock.Object,
            _actorRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _logger);

        // Act
        var movieDto = await handler.Handle(command, default);
        
        // Assert
        _movieRepositoryMock.Verify(
            x => x.Add(It.Is<Movie>(x => x.Id == movieDto.Id)), Times.Once);

    }

    [Fact]
    public async Task Handle_Should_ThrowMovieAlreadyExistsException_WhenTitleAndDirectorIsNotUnique()
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

        _movieRepositoryMock.Setup(x => x.Add(It.IsAny<Movie>()));

        _movieRepositoryMock.Setup(x => x.IsAlreadyExistAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var handler = new AddMovieCommandHandler(
            _movieRepositoryMock.Object,
            _actorRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper,
            _logger);
        
        // Act & Assert
        await Assert.ThrowsAsync<MovieAlreadyExistsException>(async () => await handler.Handle(command, default));
    }
}