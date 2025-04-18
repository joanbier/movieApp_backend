using MovieApp.Domain.Entities;

namespace MovieApp.Domain.Abstractions;

public interface IMovieRepository
{
    Task<Movie> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<bool> IsAlreadyExistAsync(string title,string director, CancellationToken cancellation = default);
    
    
    void Add(Movie movie);
    void Update(Movie movie);
    void Delete(Movie movie);
}