using Microsoft.Extensions.Logging;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Movies;

namespace MovieApp.Application.Commands.Comments.AddComment;

internal class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IUserContext _userContext;
    
    public AddCommentCommandHandler(ICommentRepository commentRepository, IMovieRepository movieRepository, 
        IUnitOfWork unitOfWork, ILogger<AddCommentCommandHandler> logger, IUserContext userContext)
    {
        _commentRepository = commentRepository;
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
        {
            throw new MovieNotFoundException(request.MovieId);
        }

        var userId = _userContext.UserId;

        if (userId == null)
        {
           throw new UnauthorizedAccessException("You are not authorized to add movie comments");
        }

        var newComment = new Comment()
        {
            Content = request.Content,
            MovieId = request.MovieId,
            UserId = userId,
        };
        
        _commentRepository.Add(newComment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogDebug($"Comment Added successfully by User with ID {userId}");
        
    }
}