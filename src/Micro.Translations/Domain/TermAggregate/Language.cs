using System.Globalization;

namespace Micro.Translations.Domain.TermAggregate;

public record Language(string Name, string Code)
{
    public static Language FromIsoCode(string isoCode)
    {
        var culture = CultureInfo.GetCultureInfo(isoCode);
        var name = culture.DisplayName;
        var code = culture.Name;
        return new Language(name, code);
    }

    public static Language EnglishAustralian() => FromIsoCode("en-AU");
    public static Language EnglishUnitedKingdom() => FromIsoCode("en-UK");
}