using Microsoft.EntityFrameworkCore;
namespace CategoryApi.Data;

public class CategoryDbContext(DbContextOptions<CategoryDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Devices and gadgets"
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                Description = "Printed and digital books"
            },
            new Category
            {
                Id = 3,
                Name = "Clothing",
                Description = "Apparel and accessories"
            },
            new Category
            {
                Id = 4,
                Name = "Home & Kitchen",
                Description = "Furniture and appliances"
            });

            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.HasKey(i => i.Id);
                b.Property(p => p.Id).HasColumnName("Id");
                b.Property(p => p.Name).HasColumnName("Name")
                    .HasColumnType("varchar(32)")
                    .IsRequired();
                b.Property(p => p.Description).HasColumnName("Description")
                    .HasMaxLength(256)
                    .IsRequired();
            });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
