namespace EFCoreRelationshipsDemo.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Passport Passport { get; set; }
}