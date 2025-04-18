using AutoMapper;
using FluentAssertions;
using Moq;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Mappings;
using MovieApp.Application.Queries.Movies.GetMovieById;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;

namespace MovieApp.UnitTests.Queries.Movies.GetMovieById;

public class GetMovieByIdQueryHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IUserFavoriteRepository> _userFavoriteRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly IMapper _mapper;

    public GetMovieByIdQueryHandlerTests()
    {
        _movieRepositoryMock = new();
        _userFavoriteRepositoryMock = new();
        _mapper = MapperHelper.CreateMapper(new MovieMappingProfile());
        _userContextMock = new();
    }

    [Fact]
    public async Task Handle_Should_CallGetByIdAsyncOnRepository_WhenGetMovieByIdQuery()
    {
        // Arrange
        _movieRepositoryMock.Setup(
                x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Movie());

        var handler = new GetMovieByIdQueryHandler(
            _movieRepositoryMock.Object,
            _userFavoriteRepositoryMock.Object,
            _mapper,
            _userContextMock.Object);

        // Act
        await handler.Handle(new GetMovieByIdQuery(1), default);

        // Assert
        _movieRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Hande_Should_ReturnMovie_WhenUserIsAnonymous()
    {
        // Arrange
        _userContextMock.Setup(x => x.UserId).Returns((string?)null);

        var movie = new Movie()
        {
            Id = 1,
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
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _movieRepositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);

        var handler = new GetMovieByIdQueryHandler(
            _movieRepositoryMock.Object,
            _userFavoriteRepositoryMock.Object,
            _mapper,
            _userContextMock.Object);

        // Act
        var movieDto = await handler.Handle(new GetMovieByIdQuery(movie.Id), default);

        // Assert
        movieDto.Should().NotBeNull();
        movieDto.Id.Should().Be(movie.Id);
        movieDto.Title.Should().Be(movie.Title);
        movieDto.ReleasedYear.Should().Be(movie.ReleasedYear);
        movieDto.Rating.Should().Be(movie.Rating);
        movieDto.Director.Should().Be(movie.Director);
        movieDto.Description.Should().Be(movie.Description);
        movieDto.Genre.Should().Be(movie.Genre);
        movieDto.PosterLink.Should().Be(movie.PosterLink);
        movieDto.Certificate.Should().Be(movie.Certificate);
        movieDto.Runtime.Should().Be(movie.Runtime);
        movieDto.Gross.Should().Be(movie.Gross);
        movieDto.IsFavorite.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenGetMovieByIdQuery()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Movie)null);
        
        
        var handler = new GetMovieByIdQueryHandler(
            _movieRepositoryMock.Object,
            _userFavoriteRepositoryMock.Object,
            _mapper,
            _userContextMock.Object);
        
        // Act
        var movieDto = await handler.Handle(new GetMovieByIdQuery(1), default);
        
        // Assert
        movieDto.Should().BeNull();
    }

}