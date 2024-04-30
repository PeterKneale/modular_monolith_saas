namespace Micro.Common.Domain;

public record UserId(Guid Value)
{
    public static UserId Create() => new(Guid.NewGuid());
    public static UserId Create(Guid guid) => new(guid);
    public static implicit operator Guid(UserId d) => d.Value;
}