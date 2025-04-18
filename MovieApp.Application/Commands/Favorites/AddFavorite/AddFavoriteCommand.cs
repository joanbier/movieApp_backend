using System.ComponentModel.DataAnnotations;
using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Favorites.AddFavorite;

public record AddFavoriteCommand(int MovieId) : ICommand
{
}