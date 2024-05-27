namespace Micro.Translations.Domain.LanguageAggregate;

public class LanguageDetail : ValueObject
{
    private LanguageDetail()
    {
        // EF Core   
    }

    private LanguageDetail(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; }
    public string Name { get; }

    public static LanguageDetail Create(string isoCode)
    {
        if (string.IsNullOrWhiteSpace(isoCode)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(isoCode));

        var culture = CultureInfo.GetCultureInfo(isoCode);
        var name = culture.DisplayName;
        var code = culture.Name;

        return new LanguageDetail(code, name);
    }

    public static implicit operator string(LanguageDetail d) => d.Code;

    public override string ToString() => Code;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}