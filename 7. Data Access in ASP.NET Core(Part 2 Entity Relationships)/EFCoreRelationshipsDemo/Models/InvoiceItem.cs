using System.Text.Json.Serialization;

namespace EFCoreRelationshipsDemo.Models
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }

        public Guid InvoiceId { get; set; } // foreign key

        [JsonIgnore]
        public Invoice? Invoice { get; set; } // reference navigation property
    }
}
