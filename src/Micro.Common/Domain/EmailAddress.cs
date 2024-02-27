namespace Micro.Common.Domain;

public record EmailAddress(string Value)
{
    public static implicit operator string(EmailAddress d) => d.Value;
}