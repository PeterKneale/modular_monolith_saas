namespace Micro.Common.Domain;

public class ProjectId : IdValueObject
{
    private ProjectId() : base()
    {
        // efcore
    }

    private ProjectId(Guid value):base(value)
    {
    }

    public static ProjectId Create() => new(Guid.NewGuid());
    public static ProjectId Create(Guid id) => new(id);

    public static implicit operator string(ProjectId d) => d.Value.ToString();
    public static implicit operator Guid(ProjectId d) => d.Value;
}