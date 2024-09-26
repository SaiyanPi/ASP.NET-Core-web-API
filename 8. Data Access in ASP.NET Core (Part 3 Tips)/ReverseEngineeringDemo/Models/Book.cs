using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Book
{
    public Guid BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public int PublicationYear { get; set; }

    public virtual ICollection<Genre> GenresGens { get; set; } = new List<Genre>();
}
