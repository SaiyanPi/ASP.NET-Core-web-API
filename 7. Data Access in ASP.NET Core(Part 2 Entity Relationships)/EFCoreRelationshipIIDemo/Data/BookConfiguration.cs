using EFCoreRelationshipIIDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipIIDemo.Data;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.BookId);
        builder.Property(b => b.BookTitle).HasColumnName("BookTitle").HasMaxLength(20).IsRequired();
        builder.HasIndex(b => b.BookTitle).IsUnique();;
        builder.Property(b => b.Author).HasColumnName("Author").HasMaxLength(20).IsRequired();
        builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(100);
        builder.Property(b => b.PublicationYear).HasColumnName("PublicationYear").IsRequired();
    }
}