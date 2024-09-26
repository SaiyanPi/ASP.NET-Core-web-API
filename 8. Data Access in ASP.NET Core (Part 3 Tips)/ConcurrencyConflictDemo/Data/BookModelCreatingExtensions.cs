using Microsoft.EntityFrameworkCore;

namespace ConcurrencyConflictDemo.Data;

public static class BookModelCreatingExtensions
{
    public static void ConfigureBook(this ModelBuilder builder)
    {
        builder.Entity<Book>()
            .Property(p => p.Version)
            .IsConcurrencyToken();
    }
}