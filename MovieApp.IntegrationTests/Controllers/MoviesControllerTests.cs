using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.Application.Commands.Movies;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;
using MovieApp.Presentation.controllers;
using Newtonsoft.Json;

namespace MovieApp.IntegrationTests.Controllers;

public class MoviesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    
    public MoviesControllerTests(WebApplicationFactory<Program> factory)
    {
        _webApplicationFactory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<MovieAppDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<MovieAppDbContext>(options => options.UseInMemoryDatabase("MovieAppDb"));
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            });
        _httpClient = _webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetById_Should_ReturnMovieAndStatusCoseOK()
    {
        // Arrange
        var scopeFactory = _webApplicationFactory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<MovieAppDbContext>();

        if (!_dbContext.Movies.Any())
        {
            _dbContext.Movies.AddRange(
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
                });
            await _dbContext.SaveChangesAsync();
        }
        
        // Act
        var response = await _httpClient.GetAsync($"/api/movies/1");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<MovieDto>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_Should_ReturnNewMovieAndStatusCodeCreated()
    {
        // Arrange
        var command = new AddMovieCommand()
        {
            Title = "The Matrix 2",
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
        
        var jsonString = JsonConvert.SerializeObject(command);
        var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        
        // Act
        var response = await _httpClient.PostAsync("/api/movies", stringContent);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<MovieDto>(content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result.Should().BeOfType<MovieDto>();

        Assert.Equal(result.Title, command.Title);
        
        Assert.Equal(result.PosterLink, command.PosterLink);

        Assert.Equal(result.ReleasedYear, command.ReleasedYear);
        
        Assert.Equal(result.Rating, command.Rating);
        
        Assert.Equal(result.Genre, command.Genre);
        
        Assert.Null(result.IsFavorite);
    }
    
    [Fact]
    public async Task Delete_Should_ReturnStatusCodeNoContent()
    {
        // Arrange
        var scopeFactory = _webApplicationFactory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<MovieAppDbContext>();

        var movie = _dbContext.Movies.Add(
            new Movie()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

        await _dbContext.SaveChangesAsync();

        int movieId = movie.Entity.Id;

        // Act
        var response = await _httpClient.DeleteAsync($"/api/movies/{movieId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}