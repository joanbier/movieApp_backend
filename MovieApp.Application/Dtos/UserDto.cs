namespace MovieApp.Application.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    // public List<string> Roles { get; set; } // Dodajemy listę ról
    public string Role { get; set; } // Dodajemy listę ról
}