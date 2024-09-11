using EFCoreRelationshipsDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreRelationshipsDemo.Data;

public class AddressConfiguration : IEntityTypeConfiguration<Passport>
{
     public void Configure(EntityTypeBuilder<Passport> builder)
    {
        builder.ToTable("Passports");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.PassportNumber).IsRequired();
        builder.Property(a => a.Country).IsRequired();
        builder.Property(a => a.PersonId).IsRequired();
        builder.HasOne(a => a.Person)
            .WithOne(c => c.Passport)
            .HasForeignKey<Passport>(a => a.PersonId);
    }
}
