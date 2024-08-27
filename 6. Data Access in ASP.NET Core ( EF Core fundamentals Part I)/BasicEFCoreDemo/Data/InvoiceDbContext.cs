using BasicEFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicEFCoreDemo.Data;

public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : DbContext(options)
{
    public DbSet<Invoice> Invoices => Set<Invoice>();

    // seeding 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-001",
                ContactName = "Iron Man",
                Description = "Invoice for the first month",
                Amount = 100,
                InvoiceDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2023, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.AwaitPayment
            },
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-002",
                ContactName = "Captain America",

                Description = "Invoice for the first month",
                Amount = 200,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.AwaitPayment
            },
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-003",
                ContactName = "Thor",
                Description = "Invoice for the first month",
                Amount = 300,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.Draft
            });

            // configuring the entity with Fluent API:
            // modelBuilder.Entity<Invoice>(b =>
            // {
            //     b.ToTable("Invoices");
            //     b.HasKey(i => i.Id);
            //     b.Property(p => p.Id).HasColumnName("Id");
            //     b.Property(p => p.InvoiceNumber).HasColumnName("InvoiceNumber").HasColumnType("varchar(32)").
            //         IsRequired();
            //     b.Property(p => p.ContactName).HasColumnName("ContactName").HasMaxLength(32).IsRequired();
            //     b.Property(p => p.Description).HasColumnName("Description").HasMaxLength(256);
            //     // b.Property(p => p.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired();
            //     b.Property(p => p.Amount).HasColumnName("Amount").HasPrecision(18, 2);
            //     b.Property(p => p.InvoiceDate).HasColumnName("InvoiceDate").HasColumnType("datetimeoffset").
            //         IsRequired();
            //     b.Property(p => p.DueDate).HasColumnName("DueDate").HasColumnType("datetimeoffset").IsRequired();
            //     b.Property(p => p.Status).HasColumnName("Status").HasMaxLength(16).HasConversion(
            //         v => v.ToString(),
            //         v => (InvoiceStatus)Enum.Parse(typeof(InvoiceStatus),
            //         v));
            // });


            // 5.3.1 using extension method to configure the entity:
            // modelBuilder.ConfigureInvoice();

            // 5.3.1 Using IEntityTypeConfiguration<TEntity> interface:
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            // or
            // new InvoiceConfiguration().Configure(modelBuilder.Entity<Invoice>());

            // Grouping the configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceDbContext).Assembly);
    }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
}


