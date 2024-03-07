using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;

namespace Micro.Translations.Application;

[ExcludeFromCodeCoverage]
public class AlreadyInUseException : PlatformException
{
    public AlreadyInUseException(TermName name) : base($"Term name '{name}' is already in use")
    {
    }

    public AlreadyInUseException(Language language) : base($"Language code {language} is already in use")
    {
    }
}