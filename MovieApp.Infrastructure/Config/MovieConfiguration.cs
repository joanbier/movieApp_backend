using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Config;

public class MovieConfiguration : BaseEntityConfiguration<Movie>
{
    public override void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.Property(m => m.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(m => m.ReleasedYear)
            .IsRequired();
        
        builder.Property(m => m.Rating)
            .HasColumnType("DECIMAL(3,1)")
            .IsRequired();
        
        builder.Property(m => m.Director)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.Property(m => m.Genre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.PosterLink)
            .HasMaxLength(200);
        
        builder.Property(m => m.Certificate)
            .HasMaxLength(10);
        
        builder.Property(m => m.Runtime)
            .HasMaxLength(10);

        builder.Property(m => m.Gross)
            .HasColumnType("DECIMAL(18,2)");
        
        builder.HasMany(m => m.MovieActors)
            .WithOne(ma => ma.Movie)
            .HasForeignKey(ma => ma.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        builder.HasMany(m => m.Comments)
            .WithOne(c => c.Movie)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}