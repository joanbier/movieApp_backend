namespace MovieApp.Domain.Entities;

public class UserFavorite
{
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public DateTime AddedAt { get; set; }
}