namespace Micro.Translations.Domain.TermAggregate;

public class TranslationText : ValueObject
{
    private const int MaxLength = 10000;

    private TranslationText(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static TranslationText Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

        if (value.Length > MaxLength) throw new ArgumentException($"Value cannot be longer than {MaxLength} characters.", nameof(value));

        return new TranslationText(value);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}