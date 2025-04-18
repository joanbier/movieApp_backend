using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.Application.Commands.Movies;

internal class AddMovieCommandHandler : ICommandHandler<AddMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    
    public AddMovieCommandHandler(IMovieRepository movieRepository, IActorRepository actorRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddMovieCommandHandler> logger)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MovieDto> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        bool isAlreadyExist = await _movieRepository.IsAlreadyExistAsync(request.Title, request.Director, cancellationToken);
        if (isAlreadyExist)
        {
            throw new MovieAlreadyExistsException(request.Title, request.Director);
        }

        var newMovie = new Movie
        {
            Title = request.Title,
            ReleasedYear = request.ReleasedYear,
            Rating = request.Rating,
            Director = request.Director,
            Description = request.Description,
            Genre = request.Genre,
            PosterLink = request.PosterLink,
            Certificate = request.Certificate,
            Runtime = request.Runtime,
            Gross = request.Gross,
        };
        
        var movieActors = new List<MovieActor>();
        
        foreach (var actorName in request.ActorNames)
        {
            var actor = await _actorRepository.GetActorByNameAsync(actorName, cancellationToken);

            if (actor == null)
            {
                actor = new Actor { Name = actorName };
                _actorRepository.Add(actor);
            }

            movieActors.Add(new MovieActor { Movie = newMovie, Actor = actor });
        }

        newMovie.MovieActors = movieActors;
        
        _movieRepository.Add(newMovie);
        await _unitOfWork.SaveChangesAsync();
        
        var movieDto = _mapper.Map<MovieDto>(newMovie);
        
        _logger.LogDebug($"Movie with ID {movieDto.Id} was added successfully.");
        
        return movieDto;
    }
    
}