using MovieApp.Domain.Abstractions;

namespace MovieApp.Domain.Entities;

public abstract class Entity : IAuditable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}