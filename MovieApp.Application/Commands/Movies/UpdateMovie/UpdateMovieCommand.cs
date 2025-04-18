using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Movies.UpdateMovie;

public class UpdateMovieCommand : ICommand
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [Required]
    [Description(description: "Year must be between 1895 and current Year")]
    public int Year { get; set; }
    
    [Required]
    [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
    public float Rating { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Director { get; set; }
    
    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Description { get; set; }
}