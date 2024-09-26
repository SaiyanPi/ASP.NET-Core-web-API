using System;
using System.Collections.Generic;

namespace ReverseEngineeringDemo.Models;

public partial class Passport
{
    public Guid Id { get; set; }

    public string PassportNumber { get; set; } = null!;

    public string Country { get; set; } = null!;

    public Guid PersonId { get; set; }

    public virtual Person Person { get; set; } = null!;
}
