namespace SecondApi2.Models;

public class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;   // reference type so non-nullable
    public string Body { get; set; } = string.Empty; 
}