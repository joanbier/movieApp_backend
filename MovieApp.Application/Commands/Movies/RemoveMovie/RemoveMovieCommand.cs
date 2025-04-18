using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Movies.RemoveMovie;

public record RemoveMovieCommand(int Id) : ICommand
{
    
}