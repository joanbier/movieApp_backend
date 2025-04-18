using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Comments.RemoveComment;

public record RemoveCommentCommand(int Id) : ICommand
{
    
}