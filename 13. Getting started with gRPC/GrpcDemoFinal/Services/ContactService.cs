using Grpc.Core;
using GrpcDemoFinal;

namespace GrpcDemo.Services;
public class ContactService(ILogger<ContactService> logger) : Contact.ContactBase
{
    public override Task<CreateContactResponse> CreateContact(CreateContactRequest request,
        ServerCallContext context)
    {
        // TODO: Save contact to database
        return Task.FromResult(new CreateContactResponse
        {
            ContactId = Guid.NewGuid().ToString()
        });
    }
}