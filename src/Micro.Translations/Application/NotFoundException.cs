using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class NotFoundException : PlatformException
{
    public NotFoundException(TermId id) : base($"{nameof(TermId)} not found {id}")
    {
    }

    public NotFoundException(TranslationId id) : base($"{nameof(TranslationId)} not found {id}")
    {
    }
}