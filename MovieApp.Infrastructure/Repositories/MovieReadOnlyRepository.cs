using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Enums;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Repositories;

internal class MovieReadOnlyRepository : IMovieReadOnlyRepository
{
    private readonly MovieAppDbContext _dbContext;

    public MovieReadOnlyRepository(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<string>> GetGenresAsync(CancellationToken cancellation = default)
    {
        var movies = await _dbContext.Movies
            .Where(m => !string.IsNullOrEmpty(m.Genre))
            .Select(m => m.Genre)
            .ToListAsync(cancellation);

        var genres = movies
            .SelectMany(genre => genre.Split(',', StringSplitOptions.TrimEntries))
            .Distinct()
            .OrderBy(g => g)
            .ToList();
        
        return genres;
    }
    
    public async Task<PagedResult<Movie>> GetAllAsync(
        int pageNumber = 1, 
        int pageSize = 10, 
        MovieSortBy sortBy = MovieSortBy.Id, 
        bool descending = false, 
        string searchTerm = "",
        string? genre = "",
        CancellationToken cancellation = default)
    {
        var query = _dbContext.Movies.AsQueryable();
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(m => m.Title.Contains(searchTerm) || 
                                     m.Description.Contains(searchTerm) || 
                                     m.Director.Contains(searchTerm) || 
                                     m.Genre.Contains(searchTerm) ||
                                     m.MovieActors.Any(ma => ma.Actor.Name.Contains(searchTerm)));
        }

        if (!string.IsNullOrEmpty(genre))
            query = query.Where(m => m.Genre != null && m.Genre.Contains(genre));

 
        query = ApplySorting(query, sortBy, descending);
        
        var totalCount = await query.CountAsync(cancellation);
        var movies = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellation);
        
        return new PagedResult<Movie>(movies, totalCount, pageNumber, pageSize);
    }
    

    private IQueryable<Movie> ApplySorting(IQueryable<Movie> query, MovieSortBy sortBy, bool descending)
    {
        switch (sortBy)
        {
            case MovieSortBy.Title:
                return descending ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title);
            case MovieSortBy.ReleasedYear:
                return descending ? query.OrderByDescending(m => m.ReleasedYear) : query.OrderBy(m => m.ReleasedYear);
            case MovieSortBy.Rating:
                return descending ? query.OrderByDescending(m => m.Rating) : query.OrderBy(m => m.Rating);
            default:
                return query.OrderBy(m => m.Id);
        }
    }
    
    
}