namespace Micro.Tenants.Domain.OrganisationAggregate;

public class OrganisationName : ValueObject
{
    private OrganisationName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static OrganisationName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Organisation Name must not be empty", nameof(value));

        value = NameSanitizer.SanitizedValue(value);

        return new OrganisationName(value);
    }

    public static implicit operator string(OrganisationName x) => x.Value;

    public override string ToString() => $"{Value}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}