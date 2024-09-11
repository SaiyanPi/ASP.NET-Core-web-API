using EFCoreRelationshipsDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipsDemo.Data;


public class ContactConfiguration : IEntityTypeConfiguration<Person>
{
     public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired();
       
    }
}