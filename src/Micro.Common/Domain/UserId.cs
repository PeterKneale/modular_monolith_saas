namespace Micro.Common.Domain;

public class UserId : IdValueObject
{
    private UserId() : base()
    {
        // efcore
    }

    private UserId(Guid value) : base(value)
    {
    }

    public static UserId Create() => new(Guid.NewGuid());
    public static UserId Create(Guid id) => new(id);

    public static implicit operator string(UserId d) => d.Value.ToString();
    public static implicit operator Guid(UserId d) => d.Value;
}