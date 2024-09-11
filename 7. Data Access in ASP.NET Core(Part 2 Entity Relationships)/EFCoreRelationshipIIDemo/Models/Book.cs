namespace EFCoreRelationshipIIDemo.Models;

public class Book
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public List<Genre> Genres { get; set; } = new(); // collection navigation property

}