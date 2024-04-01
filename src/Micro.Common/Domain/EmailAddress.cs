namespace Micro.Common.Domain;

public record EmailAddress
{
    private EmailAddress(string Value)
    {
        this.Value = Value;
    }

    public static EmailAddress Create(string Value)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException("Email address cannot be empty");
        }

        if (!Value.Contains('@'))
        {
            throw new ArgumentException("The email address is not valid, no '@' found");
        }
        if (!Value.Contains('.'))
        {
            throw new ArgumentException("The email address is not valid, no '.' found");
        }
        return new EmailAddress(Value);
    }

    public static implicit operator string(EmailAddress x) => x.Value;
    
    public string Value { get; init; }

    public bool Matches(EmailAddress emailAddress) =>
        string.Equals(emailAddress.Value, Value, StringComparison.InvariantCultureIgnoreCase);

    public override string ToString() => $"{Value}";
}