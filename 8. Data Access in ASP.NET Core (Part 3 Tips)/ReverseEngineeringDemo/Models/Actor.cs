using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Actor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}
