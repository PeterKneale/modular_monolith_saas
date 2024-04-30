using System;
using System.Collections.Generic;

namespace Micro.Translations.Infrastructure.temp;

public partial class Translation
{
    public Guid Id { get; set; }

    public Guid TermId { get; set; }

    public Guid LanguageId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Term Term { get; set; } = null!;
}
