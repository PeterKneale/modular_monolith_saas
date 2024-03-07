using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class AlreadyExistsException : PlatformException
{
    public AlreadyExistsException(TermId id) : base($"{nameof(TermId)} already exists {id}")
    {
    }

    public AlreadyExistsException(TranslationId id) : base($"{nameof(TranslationId)} already exists {id}")
    {
    }
}