namespace Micro.Common.Domain;

public record UserId(Guid Value)
{
    public static implicit operator string(UserId d) => d.Value.ToString();
    public static implicit operator Guid(UserId d) => d.Value;
    public override string ToString() => Value.ToString();
}