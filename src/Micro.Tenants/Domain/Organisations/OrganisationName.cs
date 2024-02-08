namespace Micro.Tenants.Domain.Organisations;

public record OrganisationName
{
    public OrganisationName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Organisation Name must not be empty", nameof(value));
        }
        
        Value = value;
    }

    public string Value { get; init; }
}