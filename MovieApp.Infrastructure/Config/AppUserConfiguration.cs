using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Config;

public class AppUserConfiguration :IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");
        
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.HasIndex(u => u.UserName).IsUnique();
        
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50); 
        
        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);
        
        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME")
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .HasColumnType("DATETIME")
            .IsRequired();
        
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
    }

}