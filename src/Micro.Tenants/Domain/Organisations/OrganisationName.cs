namespace Micro.Tenants.Domain.Organisations;

public class OrganisationName
{
    private OrganisationName(string value)
    {
        Value = value;
    }

    public static OrganisationName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Organisation Name must not be empty", nameof(value));
        }

        return new OrganisationName(value);
    }

    public override string ToString() => $"{Value}";

    public string Value { get; init; }
}