using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Repositories;

internal class ActorRepository :  IActorRepository
{
    private readonly MovieAppDbContext _dbContext;

    public ActorRepository(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        return await _dbContext.Actors.ToListAsync();
    }
    
    public async Task<PagedResult<Actor>> GetAllPagingAsync(int pageNumber, int pageSize, CancellationToken cancellation = default)
    {
        var totalCount = await _dbContext.Actors.CountAsync(cancellation);
        var actors = await _dbContext.Actors
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellation);
        
        return new PagedResult<Actor>(actors, totalCount, pageNumber, pageSize);
    }
    
    public async Task<PagedResult<Movie>> GetByIdWithMoviesAsync(int actorId, int pageNumber, int pageSize, CancellationToken cancellation = default)
    {
        var totalCount = await _dbContext.MovieActor
            .Where(ma => ma.ActorId == actorId)
            .CountAsync(cancellation);
        
        var movies = await _dbContext.MovieActor
            .Where(ma => ma.ActorId == actorId)
            .Select(ma => ma.Movie)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellation);
        
        return new PagedResult<Movie>(movies, totalCount, pageNumber, pageSize);
    }
    

    public async Task<Actor> GetActorByNameAsync(string actorName, CancellationToken cancellation = default)
    {
        return await _dbContext.Actors.SingleOrDefaultAsync(a => a.Name == actorName, cancellation);
    }

    public void Add(Actor actor) => _dbContext.Actors.Add(actor);
}