using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Application.Dtos;

namespace MovieApp.Application.Commands.Comments.AddComment;

public class AddCommentCommand() : ICommand
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }
    
    [Required]
    public int MovieId { get; set; }
    
}