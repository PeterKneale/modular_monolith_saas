using System;
using System.Collections.Generic;

namespace Micro.Translations.Infrastructure.temp;

public partial class Term
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
