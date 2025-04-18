using MovieApp.Domain.Common;
using MovieApp.Domain.Entities;

namespace MovieApp.Domain.Abstractions;

public interface IUserFavoriteRepository
{
    Task<bool> IsFavoriteAsync(string userId, int movieId, CancellationToken cancellation = default);
    
    Task<PagedResult<Movie>> GetAllUserFavAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellation = default);
    Task<List<int>> GetUserFavoriteMovieIdsAsync(string userId, CancellationToken cancellation = default);

    void AddFavorite(UserFavorite userFavorite);
    void RemoveFavorite(UserFavorite userFavorite);
}