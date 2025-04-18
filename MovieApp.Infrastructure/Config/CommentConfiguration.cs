using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Config;

public class CommentConfiguration : BaseEntityConfiguration<Comment>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        
        builder.Property(c => c.Content)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Movie)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        base.Configure(builder);
    }
}