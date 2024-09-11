using EFCoreRelationshipsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsDemo.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedInvoiceData(this ModelBuilder builder)
        {
            builder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = Guid.NewGuid(),
                    InvoiceNumber = "INV-001",
                    ContactName = "Iron Man",
                    Description = "Invoice for the first month",
                    Amount = 100,
                    InvoiceDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    DueDate = new DateTimeOffset(2023, 1, 15, 0, 0, 0, TimeSpan.Zero),
                    Status = InvoiceStatus.AwaitPayment
                },
                new Invoice
                {
                    Id = Guid.NewGuid(),
                    InvoiceNumber = "INV-000",
                    ContactName = "Captain America",
                    Description = "Invoice for the first month",
                    Amount = 200,
                    InvoiceDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    DueDate = new DateTimeOffset(2023, 1, 15, 0, 0, 0, TimeSpan.Zero),
                    Status = InvoiceStatus.AwaitPayment
                });
        }
    }
}
