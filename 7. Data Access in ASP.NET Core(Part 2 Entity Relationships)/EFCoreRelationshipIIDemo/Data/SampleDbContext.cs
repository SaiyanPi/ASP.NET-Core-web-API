using EFCoreRelationshipIIDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipIIDemo.Data;

public class SampleDbContext(DbContextOptions<SampleDbContext> options, IConfiguration configuration) 
: DbContext(options)
{
    // One-to-One
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Passport> Passports => Set<Passport>();


    // Many-to-Many
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Actor> Actors => Set<Actor>();

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}