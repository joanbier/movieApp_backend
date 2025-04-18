using AutoMapper;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.Application.Commands.Movies.UpdateMovie;

internal class UpdateMovieCommandHandler : ICommandHandler<UpdateMovieCommand>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id, cancellationToken);

        if (movie == null)
        {
            throw new MovieNotFoundException(request.Id);
        }
        
        movie.Title = request.Title;
        movie.ReleasedYear = request.Year;
        movie.Rating = request.Rating;
        movie.Director = request.Director;
        movie.Description = request.Description;
        
        _movieRepository.Update(movie);
        await _unitOfWork.SaveChangesAsync();
    }
}