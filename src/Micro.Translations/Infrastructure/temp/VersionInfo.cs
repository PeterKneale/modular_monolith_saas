using System;
using System.Collections.Generic;

namespace Micro.Translations.Infrastructure.temp;

public partial class VersionInfo
{
    public long Version { get; set; }

    public DateTime? AppliedOn { get; set; }

    public string? Description { get; set; }
}
