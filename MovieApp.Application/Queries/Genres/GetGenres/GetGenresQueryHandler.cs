using MediatR;
using MovieApp.Domain.Abstractions;

namespace MovieApp.Application.Queries.Genres.GetGenres;

internal class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IEnumerable<string>>
{
    
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository;

    public GetGenresQueryHandler(IMovieReadOnlyRepository movieReadOnlyRepository)
    {
        _movieReadOnlyRepository = movieReadOnlyRepository;
    }
    
    public async Task<IEnumerable<string>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _movieReadOnlyRepository.GetGenresAsync(cancellationToken);
        return genres;
    }
}