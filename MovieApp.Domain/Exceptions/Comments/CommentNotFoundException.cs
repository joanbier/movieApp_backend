using System.Net;

namespace MovieApp.Domain.Exceptions.Comments;

public class CommentNotFoundException : MovieAppException
{
    public int Id { get; set; }
    public CommentNotFoundException(int id) : base($"Comment with ID {id} was not found.")
        => Id = id;

    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}