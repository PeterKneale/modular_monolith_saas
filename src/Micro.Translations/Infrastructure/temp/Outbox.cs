using System;
using System.Collections.Generic;

namespace Micro.Translations.Infrastructure.temp;

public partial class Outbox
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string Data { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }
}
