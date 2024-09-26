using ConcurrencyConflictDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyConflictDemo.Data;

public class SampleDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public SampleDbContext(DbContextOptions<SampleDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SeedProductData();
        modelBuilder.SeedBookData();
        // native database generated concurrency token: we've used [Timestamp] attribute so no need to use following line
        // modelBuilder.ConfigureProduct();

        // application managed concurrnecy token:
        modelBuilder.ConfigureBook();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }

public DbSet<Book> Book { get; set; } = default!;
}