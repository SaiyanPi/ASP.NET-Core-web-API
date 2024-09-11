using EFCoreRelationshipIIDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipIIDemo.Data;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
     public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actors");
        builder.HasKey(a => a.Id);
        builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(32).IsRequired();
        // Add a unique index to the Name property
        builder.HasIndex(p => p.Name).IsUnique();
    }
}