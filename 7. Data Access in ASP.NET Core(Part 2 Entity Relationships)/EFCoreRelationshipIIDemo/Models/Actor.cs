using System.Text.Json.Serialization;

namespace EFCoreRelationshipIIDemo.Models;

public class Actor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Movie> Movies { get; set; } = new List<Movie>(); // collection navigation property for Movie class
    [JsonIgnore]
    public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>(); // collection navigation property for MovieActor class
}