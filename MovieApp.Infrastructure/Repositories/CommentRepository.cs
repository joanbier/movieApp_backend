using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Repositories;

internal class CommentRepository : ICommentRepository
{
    
    private readonly MovieAppDbContext _dbContext;

    public CommentRepository(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Comment> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        return await _dbContext.Comments.SingleOrDefaultAsync(m => m.Id == id, cancellation);
    }
    
    public void Add(Comment comment) => _dbContext.Comments.Add(comment);
    
    public void Delete(Comment comment) => _dbContext.Comments.Remove(comment);
    
}