namespace Micro.Common.Domain;

public record EmailAddress(string Value)
{
    public static implicit operator string(EmailAddress x) => x.Value;

    public override string ToString() => $"{Value}";
}