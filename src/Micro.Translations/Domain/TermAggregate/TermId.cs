namespace Micro.Translations.Domain.TermAggregate;

public class TermId : ValueObject
{
    private TermId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static TermId Create() => new(Guid.NewGuid());

    public static TermId Create(Guid id) => new(id);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}