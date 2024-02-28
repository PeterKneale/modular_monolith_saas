namespace Micro.Tenants.Domain.Projects;

public record ProjectName
{
    public ProjectName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Project Name must not be empty", nameof(value));
        }
        
        Value = value;
    }

    public override string ToString() => $"{Value}";
    
    public string Value { get; init; }
}