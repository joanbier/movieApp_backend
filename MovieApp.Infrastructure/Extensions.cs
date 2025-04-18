using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Auth;
using MovieApp.Infrastructure.Context;
using MovieApp.Infrastructure.Persistence;
using MovieApp.Infrastructure.Repositories;
using MovieApp.Infrastructure.Seeders.Movies;
using MovieApp.Infrastructure.Services;
using NLog.Web;

namespace MovieApp.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieReadOnlyRepository, MovieReadOnlyRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
        services.AddHostedService<CsvMovieImportHostedService>();
        services.AddScoped<ICsvMovieImporter, CsvMovieImporter>();

        
        // Pobieranie zmiennych środowiskowych z Configuration
        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
        var dbPortString = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
        var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root";
        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "MovieDB";
        
        if (!int.TryParse(dbPortString, out int dbPort))
        {
            throw new ArgumentException("DB_PORT must be a valid integer");
        }
        
        var connectionString = $"Server={dbServer};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";
        // var connectionString = configuration.GetConnectionString("MovieAppCS");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'MovieAppCS' is missing or empty.");
        }
     
        services.AddDbContext<MovieAppDbContext>(ctx => ctx.UseMySQL(connectionString));
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<MovieAppDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(1));
        
        services.AddJwtAuthentication(configuration);
        
        return services;
    }
    
    
    public static ConfigureHostBuilder UseInfrastructure(this ConfigureHostBuilder hostBuilder) 
    {
        hostBuilder.UseNLog();

        return hostBuilder;    
    }
    
}
