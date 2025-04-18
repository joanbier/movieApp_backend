using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;

namespace MovieApp.Domain.Abstractions;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<PagedResult<Actor>> GetAllPagingAsync(int pageNumber, int pageSize, CancellationToken cancellation = default);
    
    Task<PagedResult<Movie>> GetByIdWithMoviesAsync(int actorId, int pageNumber, int pageSize, CancellationToken cancellation = default);
    Task<Actor> GetActorByNameAsync(string actorName, CancellationToken cancellation = default);
    void Add(Actor actor);
}