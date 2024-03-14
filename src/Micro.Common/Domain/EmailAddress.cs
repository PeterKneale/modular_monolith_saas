namespace Micro.Common.Domain;

public record EmailAddress(string Value)
{
    public static implicit operator string(EmailAddress x) => x.Value;

    public bool Matches(EmailAddress emailAddress) =>
        string.Equals(emailAddress.Value, Value, StringComparison.InvariantCultureIgnoreCase);

    public override string ToString() => $"{Value}";
}