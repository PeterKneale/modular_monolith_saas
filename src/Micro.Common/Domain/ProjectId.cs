namespace Micro.Common.Domain;

public record ProjectId(Guid Value)
{
    public static ProjectId Create() => new(Guid.NewGuid());
    public static implicit operator string(ProjectId d) => d.Value.ToString();
    public static implicit operator Guid(ProjectId d) => d.Value;
    public override string ToString() => Value.ToString();
}