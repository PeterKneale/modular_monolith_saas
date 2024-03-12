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

    public static implicit operator string(TermId d) => d.Value.ToString();
    public static implicit operator Guid(TermId d) => d.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}