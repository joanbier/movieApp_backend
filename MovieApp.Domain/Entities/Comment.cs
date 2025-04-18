namespace MovieApp.Domain.Entities;

public class Comment : Entity
{
    public string Content { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
}

