namespace Micro.Common.Domain;

public record ProjectId(Guid Value)
{
    public static implicit operator string(ProjectId d) => d.Value.ToString();
}