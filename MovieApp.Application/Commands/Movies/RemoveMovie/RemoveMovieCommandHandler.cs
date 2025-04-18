using Microsoft.Extensions.Logging;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.Application.Commands.Movies.RemoveMovie;

internal class RemoveMovieCommandHandler : ICommandHandler<RemoveMovieCommand>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    
    public RemoveMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, ILogger<RemoveMovieCommandHandler> logger)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(RemoveMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id, cancellationToken);
        if (movie == null)
        {
            throw new MovieNotFoundException(request.Id);
        }

        _movieRepository.Delete(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Movie with ID {request.Id} was removed.");
    }
}