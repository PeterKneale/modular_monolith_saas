using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class AlreadyExistsException(TermId id) : PlatformException($"{nameof(TermId)} already exists {id}");