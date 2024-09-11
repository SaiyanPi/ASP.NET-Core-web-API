using EFCoreRelationshipIIDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipIIDemo.Data;

public class CategoryConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");
        builder.HasKey(b => b.GenId);
        builder.Property(b => b.GenTitle).HasColumnName("GenTitle").HasMaxLength(20).IsRequired();
        
       
    }
}