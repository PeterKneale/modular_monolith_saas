namespace Micro.Common.Domain;

public record ProjectId(Guid Value)
{
    public static ProjectId Create()=> new ProjectId(Guid.NewGuid());
    public static implicit operator string(ProjectId d) => d.Value.ToString();
}