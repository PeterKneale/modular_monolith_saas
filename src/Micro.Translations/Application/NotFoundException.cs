using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class NotFoundException : PlatformException
{
    public NotFoundException(LanguageId id) : base($"{nameof(LanguageId)} not found {id}")
    {
    }

    public NotFoundException(TermId id) : base($"{nameof(TermId)} not found {id}")
    {
    }

    public NotFoundException(TranslationId id) : base($"{nameof(TranslationId)} not found {id}")
    {
    }
}