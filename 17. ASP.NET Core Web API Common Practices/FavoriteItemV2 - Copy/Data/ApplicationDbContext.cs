using FavoriteItemV2.Authentication;
using FavoriteItemV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FavoriteItemV2.Data;

// using one ApplicationDbContext that inherits from IdentityDbContext<AppUser> and includes both
// Identity and domain models.
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
: IdentityDbContext<AppUser>(options)
{
    // many-to-many
    public DbSet<Item> Items { get; set; }
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
            .HasKey(ufi => ufi.Id); // Or configure composite key if you prefer UserId + ItemId

        modelBuilder.Entity<UserFavouriteItem>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.FavouriteItem)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: set delete behavior

        modelBuilder.Entity<UserFavouriteItem>()
            .HasOne(uf => uf.Item)
            .WithMany(i => i.UserFavourite)
            .HasForeignKey(uf => uf.ItemId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: set delete behavior
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}