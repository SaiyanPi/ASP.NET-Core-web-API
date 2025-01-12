using InvoiceApp.WebApi.Models;

namespace InvoiceApp.WebApi.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetContactsAsync(int page = 1, int pageSize = 10);
        Task<Contact?> GetContactAsync(Guid id);
        Task<Contact> CreateContactAsync(Contact contact);
        Task<Contact?> UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(Guid id);

    }
}
