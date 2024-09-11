namespace EFCoreRelationshipsDemo.Models;

public class Passport
{
    public Guid Id { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Guid PersonId { get; set; } // foreign key
    public Person Person {get; set; }

}