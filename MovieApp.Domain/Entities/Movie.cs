namespace MovieApp.Domain.Entities;

public class Movie : Entity
{
    public string Title { get; set; }
    public int ReleasedYear { get; set; }
    public float Rating { get; set; }
    public string Director { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string PosterLink { get; set; }
    public string Certificate { get; set; }
    public string Runtime { get; set; }
    public decimal Gross { get; set; }
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserFavorite> FavoritedBy { get; set; } = new List<UserFavorite>();
}