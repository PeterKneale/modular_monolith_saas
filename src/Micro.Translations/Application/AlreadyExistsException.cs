using Micro.Translations.Domain.TermAggregate;

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