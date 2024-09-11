using System.Text.Json.Serialization;

namespace EFCoreRelationshipIIDemo.Models;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ReleaseYear { get; set; }
    public List<Actor> Actors { get; set; } = new List<Actor>(); // collection navigation property for Actor class
    [JsonIgnore]
    public List<MovieActor> MovieActors { get; set; } = new(); // collection navigation property for MovieActor class
}