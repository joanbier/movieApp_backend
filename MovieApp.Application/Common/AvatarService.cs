using System.Security.Cryptography;
using System.Text;
using MovieApp.Domain.Enums;

namespace MovieApp.Application.Common;

public static class AvatarService
{
    public static string GenerateGravatarUrl(string email, AvatarType newAvatarType = AvatarType.RoboHash)
    {
        if (string.IsNullOrEmpty(email))
        {
            return "https://www.gravatar.com/avatar/00000000000000000000000000000000?s=150&d=mp";
        }

        using var md5 = MD5.Create();
        var emailBytes = Encoding.UTF8.GetBytes(email.Trim().ToLower());
        var hashBytes = md5.ComputeHash(emailBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        var avatarType = newAvatarType.ToString().ToLower();

        return $"https://www.gravatar.com/avatar/{hash}?s=150&d={avatarType}";
    }
    
    public static string ChangeAvatarType(string email, AvatarType newAvatarType)
    {
        return GenerateGravatarUrl(email, newAvatarType);
    }


}