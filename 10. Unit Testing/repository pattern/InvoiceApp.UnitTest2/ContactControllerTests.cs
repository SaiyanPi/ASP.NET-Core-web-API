using InvoiceApp.WebApi;
using InvoiceApp.WebApi.Controllers;
using InvoiceApp.WebApi.Interfaces;
using InvoiceApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.UnitTest2
{
    public class ContactControllerTests(TestFixture fixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task GetContacts_ShouldReturnContacts()
        {
            // Arrange
            var contactsRepositoryMock = new Mock<IContactRepository>();

            contactsRepositoryMock.Setup(x => x.GetContactsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int pageSize) =>
                    fixture.Contacts
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList());

            var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            var controller = new ContactController(contactsRepositoryMock.Object, invoiceRepositoryMock.Object);
            // Act
            var actionResult = await controller.GetContactsAsync();
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            var returnResult = Assert.IsAssignableFrom<List<Contact>>(result.Value);
            Assert.NotNull(returnResult);
            Assert.Equal(2, returnResult.Count);
            Assert.Contains(returnResult, c => c.FirstName == "John");
            Assert.Contains(returnResult, c => c.FirstName == "Jane");
        }

        [Fact]
        public async Task GetContact_ShouldReturnContact()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();

            contactRepositoryMock.Setup(x => x.GetContactAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => fixture.Contacts.FirstOrDefault(x => x.Id == id));

            var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            var controller = new ContactController(contactRepositoryMock.Object, invoiceRepositoryMock.Object);
            // Act
            var contact = fixture.Contacts.First();
            var actionResult = await controller.GetContactAsync(contact.Id);
            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            var returnResult = Assert.IsAssignableFrom<Contact>(result.Value);
            Assert.NotNull(returnResult);
            Assert.Equal(contact.Id, returnResult.Id);
            Assert.Equal(contact.FirstName, returnResult.FirstName);
        }
    }
}
