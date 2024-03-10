namespace Micro.Common.Domain;

public record Password(string Value)
{
    public static implicit operator string(Password x) => x.Value;

    public override string ToString() => "*******";
}