using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual Passport? Passport { get; set; }
}
