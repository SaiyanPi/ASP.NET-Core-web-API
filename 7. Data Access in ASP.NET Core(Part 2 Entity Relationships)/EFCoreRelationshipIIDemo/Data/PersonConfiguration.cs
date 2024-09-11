using EFCoreRelationshipIIDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipIIDemo.Data;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
     public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired();
       
    }
}