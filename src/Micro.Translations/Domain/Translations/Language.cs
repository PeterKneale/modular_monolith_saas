using System.Globalization;

namespace Micro.Translations.Domain.Translations;

public class Language
{
    public string Name { get; }

    private Language(string name)
    {
        Name = name;
    }

    public static Language FromName(string name)
    {
        var culture = CultureInfo.GetCultureInfo(name);
        return new Language(culture.Name);
    }
}