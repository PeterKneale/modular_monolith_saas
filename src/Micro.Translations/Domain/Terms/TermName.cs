namespace Micro.Translations.Domain.Terms;

public class TermName(string value)
{
    public string Value { get; } = value;

    public static TermName Create(string value) => new TermName(value);
}