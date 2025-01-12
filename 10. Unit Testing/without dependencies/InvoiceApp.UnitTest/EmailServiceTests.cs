using InvoiceApp.WebApi;
using InvoiceApp.WebApi.Models;
using InvoiceApp.WebApi.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvoiceApp.UnitTests;

public class EmailServiceTests
{

    [Fact]
    public void GenerateInvoiceEmail_Should_Return_Email()
    {
        var invoiceDate = DateTimeOffset.Now;
        var dueDate = invoiceDate.AddDays(30);
        // Arrange
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = "INV-001",
            Amount = 500,
            DueDate = dueDate,
            Contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            },
            Status = InvoiceStatus.Draft,
            InvoiceDate = invoiceDate,
            InvoiceItems = new List<InvoiceItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Description = "Item 1",
                    Quantity = 1,
                    UnitPrice = 100,
                    Amount = 100
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Description = "Item 2",
                    Quantity = 2,
                    UnitPrice = 200,
                    Amount = 400
                }
            }
        };
        // Act
        var (to, subject, body) = new EmailService().GenerateInvoiceEmail(invoice);

        // Assert
        Assert.Equal(invoice.Contact.Email, to);
        Assert.Equal($"Invoice INV-001 for John Doe", subject);
        Assert.Equal($"""
            Dear John Doe,

            Thank you for your business. Here are your invoice details:
            Invoice Number: INV-001
            Invoice Date: {invoiceDate.LocalDateTime.ToShortDateString()}
            Invoice Amount: {invoice.Amount.ToString("C")}
            Invoice Items:
            Item 1 - 1 x $100.00
            Item 2 - 2 x $200.00

            Please pay by {invoice.DueDate.LocalDateTime.ToShortDateString()}. Thank you!

            Regards,
            InvoiceApp
            """, body);
    }
}