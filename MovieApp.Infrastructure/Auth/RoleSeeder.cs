using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MovieApp.Infrastructure.Auth;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = ["Admin", "User"];
        
        var existingRoles = roleManager.Roles.Select(r => r.Name).ToHashSet();

        foreach (var roleName in roleNames)
        {
            if (!existingRoles.Contains(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}