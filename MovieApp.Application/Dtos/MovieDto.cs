
namespace MovieApp.Application.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PosterLink { get; set; }
    public int ReleasedYear { get; set; }
    public float Rating { get; set; }
    public string Genre { get; set; }
    public bool? IsFavorite { get; set; }
}