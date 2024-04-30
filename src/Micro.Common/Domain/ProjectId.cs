namespace Micro.Common.Domain;

public record ProjectId(Guid Value)
{
    public static ProjectId Empty => new(Guid.Empty);
    public static ProjectId Create() => new(Guid.NewGuid());
    public static ProjectId Create(Guid guid) => new(guid);
    public static ProjectId Create(Guid? guid) => guid == null ? Empty : Create(guid.Value);
    public static implicit operator Guid(ProjectId d) => d.Value;
}