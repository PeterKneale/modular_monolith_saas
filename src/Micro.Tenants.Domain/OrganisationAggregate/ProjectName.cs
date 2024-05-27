namespace Micro.Tenants.Domain.OrganisationAggregate;

public record ProjectName
{
    private ProjectName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ProjectName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Project Name must not be empty", nameof(value));

        value = NameSanitizer.SanitizedValue(value);

        return new ProjectName(value);
    }

    public static implicit operator string(ProjectName d) => d.Value;

    public override string ToString() => $"{Value}";
}