using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Enums;

namespace MovieApp.Domain.Abstractions;

public interface IMovieReadOnlyRepository
{
    Task<PagedResult<Movie>> GetAllAsync(
        int pageNumber, 
        int pageSize, 
        MovieSortBy sortBy, 
        bool descending, 
        string search = "", 
        string genre = "",
        CancellationToken cancellation = default);
    
    Task<List<string>> GetGenresAsync(CancellationToken cancellation = default);
}
