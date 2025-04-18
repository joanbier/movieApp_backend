using MovieApp.Domain.Entities;

namespace MovieApp.Domain.Abstractions;

public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(int id, CancellationToken cancellation = default);
    void Add(Comment comment);
    void Delete(Comment comment);
}