using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Context;

namespace MovieApp.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly MovieAppDbContext _dbContext;

    public UnitOfWork(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateAuditableEntities()
    {
        var entries = _dbContext
            .ChangeTracker
            .Entries<Entity>();

        foreach (var entry in entries) 
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = entry.Entity.UpdatedAt = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}