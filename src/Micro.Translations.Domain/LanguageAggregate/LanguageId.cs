namespace Micro.Translations.Domain.LanguageAggregate;

public class LanguageId : ValueObject
{
    private LanguageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static LanguageId Create() => new(Guid.NewGuid());

    public static LanguageId Create(Guid id) => new(id);

    public override string ToString() => Value.ToString();

    public static implicit operator string(LanguageId d) => d.Value.ToString();
    public static implicit operator Guid(LanguageId d) => d.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}