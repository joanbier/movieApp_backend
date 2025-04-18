using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Domain.Entities;

namespace MovieApp.Infrastructure.Config;

public class ActorConfiguration : BaseEntityConfiguration<Actor>
{
    public override void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actors");
        
        builder.Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasMany(a => a.MovieActors)
            .WithOne(ma => ma.Actor)
            .HasForeignKey(ma => ma.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}