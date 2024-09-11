namespace EFCoreRelationshipIIDemo.Models;

public class Genre
{
    public Guid GenId { get; set; }
    public string GenTitle { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = new(); // collection navigation property
}