using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Config;

public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
{
    public void Configure(EntityTypeBuilder<UserFavorite> builder)
    {
        builder.ToTable("UserFavorites");
        
        builder.HasKey(uf => new { uf.UserId, uf.MovieId });

        builder.HasOne(uf => uf.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Movie)
            .WithMany(m => m.FavoritedBy)
            .HasForeignKey(uf => uf.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(uf => uf.AddedAt)
            .HasColumnType("DATETIME");

    }
}