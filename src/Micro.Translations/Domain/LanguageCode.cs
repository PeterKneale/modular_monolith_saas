using System.Globalization;

namespace Micro.Translations.Domain;

public record LanguageCode(string Name, string Code)
{
    public static LanguageCode FromIsoCode(string isoCode)
    {
        var culture = CultureInfo.GetCultureInfo(isoCode);
        var name = culture.DisplayName;
        var code = culture.Name;
        return new LanguageCode(name, code);
    }
}