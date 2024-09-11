using EFCoreRelationshipsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsDemo.Data;

public class SampleDbContext(DbContextOptions<SampleDbContext> options, IConfiguration configuration) : DbContext(options)
{
    // one-to-many without nullable foreign key()
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    // many-to-many with nullable foreign key
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Post> Posts => Set<Post>();

    // one-to-one
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Passport> Passports => Set<Passport>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SeedInvoiceData();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}

    

