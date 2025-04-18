namespace MovieApp.Infrastructure.Seeders.Movies;

public class MovieCsvDto
{
    public string PosterLink { get; set; }
    public string Title { get; set; }
    public int ReleasedYear { get; set; }
    public string? Certificate { get; set; }
    public string Runtime { get; set; }
    public string Genre { get; set; }
    public float Rating { get; set; }
    public string Description { get; set; }
    public int? MetaScore { get; set; }
    public string Director { get; set; }
    public string Star1 { get; set; }
    public string Star2 { get; set; }
    public string Star3 { get; set; }
    public string Star4 { get; set; }
    public int NoOfVotes { get; set; }
    public string? Gross { get; set; }
}