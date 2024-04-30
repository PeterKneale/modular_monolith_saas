namespace Micro.Common.Domain;

public class IdValueObject : ValueObject
{
    protected IdValueObject(Guid value)
    {
        Value = value;
    }

    protected IdValueObject()
    {
        // ef core
    }

    public Guid Value { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value.ToString();
}