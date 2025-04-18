using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Application.Dtos;

namespace MovieApp.Application.Commands.Movies;

public class AddMovieCommand : ICommand<MovieDto>
{
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [Required]
    [Description(description: "ReleasedYear must be between 1895 and current Year")]
    public int ReleasedYear { get; set; }
    
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
    
    [Required]
    [MaxLength(100)]
    public string Genre { get; set; }
    
    [MaxLength(200)]
    public string PosterLink { get; set; }
    
    [MaxLength(10)]
    public string Certificate { get; set; }
    
    [MaxLength(10)]
    public string Runtime { get; set; }
    
    public decimal Gross  { get; set; }
    
    public List<string> ActorNames { get; set; }
}