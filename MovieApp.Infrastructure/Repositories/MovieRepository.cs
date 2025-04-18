using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Repositories;

internal class MovieRepository : IMovieRepository
{
    
    private readonly MovieAppDbContext _dbContext;

    public MovieRepository(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<Movie> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        return await _dbContext.Movies
            .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
            .Include(c => c.Comments)
                .ThenInclude(c => c.User)
            .Include(m => m.FavoritedBy)
            .SingleOrDefaultAsync(m => m.Id == id, cancellation);
    }

    public async Task<bool> IsAlreadyExistAsync(string title, string director, CancellationToken cancellation = default)
    {
        return await _dbContext.Movies.AnyAsync(x => x.Title == title && x.Director == director, cancellation);
    }

    public void Add(Movie movie)
    {
        _dbContext.Movies.Add(movie);
    }

    public void Update(Movie movie)
    {
        _dbContext.Movies.Update(movie);
    }

    public void Delete(Movie movie)
    {
        _dbContext.Movies.Remove(movie);
    }
}