using System.Text.Json.Serialization;
using EFCoreRelationshipsDemo.Models;


public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; } // foreign key
    
    [JsonIgnore]
    public Category? Category { get; set; } // reference navigation property
}