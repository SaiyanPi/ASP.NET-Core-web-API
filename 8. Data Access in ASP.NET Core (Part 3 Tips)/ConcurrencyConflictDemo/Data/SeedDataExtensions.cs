using ConcurrencyConflictDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyConflictDemo.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedProductData(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product{
                    Id = 1,
                    Name = "Product 1",
                    Description = "Product 1 Description",
                    Price = 10m,
                    Inventory = 15
                },
                new Product{
                    Id = 2,
                    Name = "Product 2",
                    Description = "Product 2 Description",
                    Price = 20m,
                    Inventory = 20
                }
            );
        }

        public static void SeedBookData(this ModelBuilder builder)
        {
            builder.Entity<Book>().HasData(
                new Book{
                    Id = 1,
                    Name = "Minds",
                    Author = "John Mckenny",
                    Quantity = 15,
                    Version = Guid.NewGuid()
                }
            );
        }
    }
}
