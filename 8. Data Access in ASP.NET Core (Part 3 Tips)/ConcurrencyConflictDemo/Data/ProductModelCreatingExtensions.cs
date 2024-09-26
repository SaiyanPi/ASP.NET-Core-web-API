using ConcurrencyConflictDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyConflictDemo.Data;

public static class InvoiceModelCreatingExtensions
{
    public static void ConfigureProduct(this ModelBuilder builder)
    {
        builder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
    }
}