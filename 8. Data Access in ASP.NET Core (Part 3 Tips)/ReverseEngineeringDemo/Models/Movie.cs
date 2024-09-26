using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Movie
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int ReleaseYear { get; set; }

    public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}
