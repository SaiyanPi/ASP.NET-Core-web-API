using EfCoreDemo.Data;
using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace EFCoreDemo.Data;

public class SampleDbContext(DbContextOptions<SampleDbContext> options) : DbContext(options)
{
    public DbSet<Invoice> Invoices => Set<Invoice>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // extension method to configure the entity
        modelBuilder.ConfigureInvoice();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // disabling tracking globally
        // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}