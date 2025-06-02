using Microsoft.EntityFrameworkCore;
using OutputCaching.Models;

namespace OutputCaching.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
: DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>().HasData(
            new Tenant
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Tenant 1",
                Domain = "tenant1.example.com"
            },
            new Tenant
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Tenant 2",
                Domain = "tenant2.example.com"
            },
            new Tenant
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Tenant 3",
                Domain = "tenant3.example.com"
            },
            new Tenant
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name = "Tenant 4",
                Domain = "tenant4.example.com"
            });

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
} 