using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Quantity { get; set; }
    
    // Add a new property as the concurrency token
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}