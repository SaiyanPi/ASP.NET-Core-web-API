using FavouriteItemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FavouriteItemApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
: DbContext(options)
{
    // many-to-many
    public DbSet<Item> Items { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<UserFavouriteItem> UserFavouriteItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // data seeding
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "laptop", Description = "This is a laptop" },
            new Item { Id = 2, Name = "Fabric", Description = "This is a fabric" },
            new Item { Id = 3, Name = "Phones", Description = "This is a phone" },
            new Item { Id = 4, Name = "Shoes", Description = "This is a shoe" }
        );

        // configuring the many-to-many relationship
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFavouriteItem>()
            .HasKey(uf => new { uf.UserId, uf.ItemId }); // composite key

        modelBuilder.Entity<UserFavouriteItem>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.FavouriteItem)
            .HasForeignKey(uf => uf.UserId);

        modelBuilder.Entity<UserFavouriteItem>()
            .HasOne(uf => uf.Item)
            .WithMany(i => i.UserFavourite)
            .HasForeignKey(uf => uf.ItemId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}