using MediatR;

namespace MovieApp.Application.Queries.Genres.GetGenres;

public record GetGenresQuery : IRequest<IEnumerable<string>>
{
    
}