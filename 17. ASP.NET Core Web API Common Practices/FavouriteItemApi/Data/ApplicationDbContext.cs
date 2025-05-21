using FavouriteItemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FavouriteItemApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
: DbContext(options)
{
    // many-to-many
    public DbSet<Item> Items { get; set; }
    public DbSet<UserFavouriteItem> UserFavouriteItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFavouriteItem>()
            .HasKey(uf => new { uf.UserId, uf.ItemId });

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