using AutoMapper;
using FluentAssertions;
using MovieApp.Domain.Abstractions;
using Moq;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Mappings;
using MovieApp.Application.Queries.Movies.GetMovies;
using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Enums;

namespace MovieApp.UnitTests.Queries.Movies.GetMovies;

public class GetMoviesQueryHandlerTests
{
    private readonly Mock<IMovieReadOnlyRepository> _movieReadOnlyRepositoryMock;
    private readonly Mock<IUserFavoriteRepository> _userFavoriteRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly IMapper _mapper;

    public GetMoviesQueryHandlerTests()
    {
        _movieReadOnlyRepositoryMock = new();
        _userFavoriteRepositoryMock = new();
        _mapper = MapperHelper.CreateMapper(new MovieMappingProfile());
        _userContextMock = new();
    }

    [Fact]
    public async Task Handle_Should_CallGetAllAsyncOnRepository_WhenGetMoviesQuery()
    {
        //Arrange
        var movies = new List<Movie>(); 
        var pagedResult = new PagedResult<Movie>(movies, totalCount: 0, pageNumber: 1, pageSize: 10);
        
        _movieReadOnlyRepositoryMock.Setup(
            x => x.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<MovieSortBy>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(pagedResult);
        
        var handler = new GetMoviesQueryHandler(
            _movieReadOnlyRepositoryMock.Object, 
            _userFavoriteRepositoryMock.Object, 
            _mapper,
            _userContextMock.Object);
        
        // Act
        var query = new GetMoviesQuery(
            PageNumber: 1,
            PageSize: 10,
            SortBy: MovieSortBy.Id,
            Descending: false,
            SearchTerm: "",
            Genre: ""
        );
        
        await handler.Handle(query, default);
        
        // Assert
        _movieReadOnlyRepositoryMock.Verify(x => x.GetAllAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<MovieSortBy>(),
            It.IsAny<bool>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedMovies_WhenUserIsAnonymous()
    {
        // Arrange 
        _userContextMock.Setup(x => x.UserId).Returns((string?)null);
        
        var movies = new List<Movie>()
        {
            new Movie()
            {
                Id = 1,
                Title = "The Matrix",
                ReleasedYear = 1999,
                Rating = 8.7f,
                Director = "Lana Wachowski",
                Description = "Neo discovers the elaborate deception of an evil cyber-intelligence.",
                Genre = "Action, Sci-Fi",
                PosterLink = "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@.jpg",
                Certificate = "A",
                Runtime = "136 min",
                Gross = 171479930,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Movie()
            {
                Id = 2,
                Title = "Inception",
                ReleasedYear = 2010,
                Rating = 8.8f,
                Director = "Christopher Nolan",
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology...",
                Genre = "Action, Adventure, Sci-Fi",
                PosterLink = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@.jpg",
                Certificate = "UA",
                Runtime = "148 min",
                Gross = 292576195,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        var pagedResult = new PagedResult<Movie>(
            movies,
            totalCount: movies.Count,
            pageNumber: 1,
            pageSize: 10
        );
        
        _movieReadOnlyRepositoryMock.Setup(x => x.GetAllAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<MovieSortBy>(),
            It.IsAny<bool>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(pagedResult);
        
        var handler = new GetMoviesQueryHandler(
            _movieReadOnlyRepositoryMock.Object, 
            _userFavoriteRepositoryMock.Object, 
            _mapper,
            _userContextMock.Object);
        
        // Act
        var query = new GetMoviesQuery(
            PageNumber: 1,
            PageSize: 10,
            SortBy: MovieSortBy.Id,
            Descending: false,
            SearchTerm: "",
            Genre: ""
        );
        
        var moviesDto = await handler.Handle(query, default);
        
        // Assert
        moviesDto.Should().NotBeNull();
        moviesDto.Items.Should().NotBeNull();
        moviesDto.Items.Should().HaveCount(2);
        moviesDto.Items.Should().OnlyContain(m => !string.IsNullOrWhiteSpace(m.Title));
        moviesDto.Items.Select(m => m.Title).Should().Contain("Inception");
        moviesDto.Items.Should().OnlyContain(m => m.IsFavorite == null);
        moviesDto.TotalCount.Should().Be(2);  

    }
    
}