using FluentAssertions;
using InvoiceApp.WebApi.Controllers;
using InvoiceApp.WebApi.Models;
using InvoiceApp.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.UnitTest
{
    public class InvoiceControllerTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
    {
        [Fact]
        public async Task GetInvoices_ShouldReturnInvoices()
        {
            // Arrange
            await using var dbContext = fixture.CreateDbContext();
            var emailServiceMock = new Mock<IEmailService>();
            var controller = new InvoiceController(dbContext, emailServiceMock.Object);
            // Act
            var actionResult = await controller.GetInvoicesAsync();
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            var returnResult = Assert.IsAssignableFrom<List<Invoice>>(result.Value);
            //Assert.NotNull(returnResult);
            //Assert.Equal(2, returnResult.Count);
            //Assert.Contains(returnResult, i => i.InvoiceNumber == "INV-001");
            //Assert.Contains(returnResult, i => i.InvoiceNumber == "INV-002");
            returnResult.Should().NotBeNull();
            //returnResult.Should().HaveCount(2);
            returnResult.Count.Should().Be(2, "The number of invoices should be 2");
            returnResult.Should().Contain(i => i.InvoiceNumber == "INV-001");
            returnResult.Should().Contain(i => i.InvoiceNumber == "INV-002");
        }

    }
}
