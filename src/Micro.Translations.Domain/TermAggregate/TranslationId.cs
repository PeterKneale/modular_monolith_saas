namespace Micro.Translations.Domain.TermAggregate;

public class TranslationId : ValueObject
{
    private TranslationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static TranslationId Create() => new(Guid.NewGuid());

    public static TranslationId Create(Guid id) => new(id);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}