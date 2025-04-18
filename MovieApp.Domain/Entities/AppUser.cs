using Microsoft.AspNetCore.Identity;
using MovieApp.Domain.Abstractions;

namespace MovieApp.Domain.Entities;

public class AppUser : IdentityUser, IAuditable
{
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
    
}