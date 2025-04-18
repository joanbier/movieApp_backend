using MovieApp.Application.Configuration.Commands;

namespace MovieApp.Application.Commands.Favorites.RemoveFavorite;

public record RemoveFavoriteCommand(int MovieId) : ICommand;