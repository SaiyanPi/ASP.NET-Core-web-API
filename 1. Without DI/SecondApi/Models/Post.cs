namespace SecondApi.Models;

public class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;   // reference type so non-nullable
    // explicitly making making reference type property nullable:
    // public string? Message { get; set; }
    public string Body { get; set; } = string.Empty;
    
   
    // WHY 'string.Empty;' ?
    // if only 'public string Body { get; set; }'
    // error: Non-nullable property 'UserId' must contain a non-null value when exiting constructor.
    // Consider adding the 'required' modifier or declaring the property as nullable.
}
