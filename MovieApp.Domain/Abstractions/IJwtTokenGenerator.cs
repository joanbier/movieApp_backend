using MovieApp.Domain.Entities;

namespace MovieApp.Domain.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user, IList<string> roles);
}