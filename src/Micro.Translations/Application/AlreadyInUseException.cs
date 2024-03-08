using Micro.Translations.Domain.TermAggregate;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class AlreadyInUseException(TermName name) : PlatformException($"Term name '{name}' is already in use");