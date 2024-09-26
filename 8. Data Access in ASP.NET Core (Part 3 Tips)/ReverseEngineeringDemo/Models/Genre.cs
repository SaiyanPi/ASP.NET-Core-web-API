using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Genre
{
    public Guid GenId { get; set; }

    public string GenTitle { get; set; } = null!;

    public virtual ICollection<Book> BooksBooks { get; set; } = new List<Book>();
}
