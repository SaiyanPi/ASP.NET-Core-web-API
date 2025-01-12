using InvoiceApp.WebApi.Data;
using InvoiceApp.WebApi;
using Moq;

using InvoiceApp.IntegrationTests.SeedHelper;
using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.WebApi.Interfaces;
using Microsoft.AspNetCore.TestHost;
using FluentAssertions;

namespace InvoiceApp.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class EmailServiceTestWithMockServices(IntegrationTestingDatabaseFixture factory) : IDisposable
    {

        [Theory]
        [InlineData("7e096984-5919-492c-8d4f-ce93f25eaed5")]
        [InlineData("b1ca459c-6874-4f2b-bc9d-f3a45a9120e4")]
        public async Task SendInvoiceAsync_ReturnsSuccessAndCorrectContentType(string invoiceId)
        {
            // Arrange
                /// creating a mock email sender service
            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask).Verifiable();
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    /// finding the registered IEmailSender service
                    var emailSender = services.SingleOrDefault(x => x.ServiceType == typeof(IEmailSender));
                    /// removing
                    services.Remove(emailSender);
                    /// adding our mock service to the service collection
                    services.AddScoped<IEmailSender>(_ => mockEmailSender.Object);
                });
            }).CreateClient();
            // Act
            var response = await client.PostAsync($"/api/invoice/{invoiceId}/send", null);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            mockEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            var scope = factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<InvoiceDbContext>();
            var invoice = await db.Invoices.FindAsync(Guid.Parse(invoiceId));
            invoice!.Status.Should().Be(InvoiceStatus.AwaitPayment);
        }

        public void Dispose()
        {
            // Clean up the database
            var scope = factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<InvoiceDbContext>();
            Utilities.Cleanup(db);
        }
    }
}
