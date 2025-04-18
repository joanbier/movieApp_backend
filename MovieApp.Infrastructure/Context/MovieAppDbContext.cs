using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApp.Domain.Entities;
using MovieApp.Infrastructure.Config;

namespace MovieApp.Infrastructure.Context;


internal class MovieAppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieActor> MovieActor { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFavorite> UserFavorite { get; set; }

    public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new MovieConfiguration());
        modelBuilder.ApplyConfiguration(new ActorConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        modelBuilder.ApplyConfiguration(new MovieActorConfiguration());
        modelBuilder.ApplyConfiguration(new UserFavoriteConfiguration());
    
    }
}