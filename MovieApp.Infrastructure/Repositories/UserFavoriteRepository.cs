using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Repositories;

internal class UserFavoriteRepository : IUserFavoriteRepository
{
    private readonly MovieAppDbContext _dbContext;

    public UserFavoriteRepository(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsFavoriteAsync(string userId, int movieId, CancellationToken cancellation = default)
    {
        return await _dbContext.UserFavorite.AnyAsync(x => x.UserId == userId && x.MovieId == movieId, cancellation);
    }

    public async Task<PagedResult<Movie>> GetAllUserFavAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellation = default)
    {
        var totalCount = await _dbContext.UserFavorite
            .Where(x => x.UserId == userId)
            .CountAsync(cancellation);
        
        var movies = await _dbContext.UserFavorite
            .Where(x => x.UserId == userId)
            .Select(x => x.Movie)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellation);
        
        return new PagedResult<Movie>(movies, totalCount, pageNumber, pageSize);
    }

    public async Task<List<int>> GetUserFavoriteMovieIdsAsync(string userId, CancellationToken cancellation = default)
    {
        return await _dbContext.UserFavorite.Where(x => x.UserId == userId).Select(x => x.MovieId).ToListAsync(cancellation);
    }
    

    public void AddFavorite(UserFavorite userFavorite)
    {
        _dbContext.UserFavorite.Add(userFavorite);
    }

    public void RemoveFavorite(UserFavorite userFavorite)
    {
        _dbContext.UserFavorite.Remove(userFavorite);
    }
}