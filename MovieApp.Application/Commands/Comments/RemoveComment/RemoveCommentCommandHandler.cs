using Microsoft.Extensions.Logging;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Exceptions.Comments;

namespace MovieApp.Application.Commands.Comments.RemoveComment;

internal class RemoveCommentCommandHandler : ICommandHandler<RemoveCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    
    public RemoveCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, ILogger<RemoveCommentCommandHandler> logger)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    
    public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (comment == null)
        {
            throw new CommentNotFoundException(request.Id);
        }

        _commentRepository.Delete(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Comment with ID {request.Id} was removed.");
    }
}