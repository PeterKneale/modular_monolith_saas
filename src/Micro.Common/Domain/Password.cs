namespace Micro.Common.Domain;

public record Password(string Value)
{
    public static implicit operator string(Password d) => d.Value;
}