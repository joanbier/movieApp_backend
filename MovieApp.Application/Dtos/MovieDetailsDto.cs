namespace MovieApp.Application.Dtos;

public class MovieDetailsDto : MovieDto
{
    public string Director { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string Certificate { get; set; }
    public string Runtime { get; set; }
    public decimal Gross { get; set; }
    public List<ActorDto> Actors { get; set; }
    public List<CommentDto> Comments { get; set; }
    public int Likes { get; set; }
}