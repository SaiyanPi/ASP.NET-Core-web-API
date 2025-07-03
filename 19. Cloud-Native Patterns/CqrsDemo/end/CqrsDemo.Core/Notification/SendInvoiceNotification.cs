using MediatR;

namespace CqrsDemo.Core.Notification;
public class SendInvoiceNotification(Guid invoiceId) : INotification
{
    public Guid InvoiceId { get; set; } = invoiceId;
}