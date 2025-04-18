namespace MovieApp.Application.Dtos;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string? UserName { get; set; }
}