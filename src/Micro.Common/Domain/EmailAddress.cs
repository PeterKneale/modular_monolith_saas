namespace Micro.Common.Domain;

public record EmailAddress
{
    private EmailAddress(string Value)
    {
        this.Value = Value;
    }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty");
        }

        if (!value.Contains('@'))
        {
            throw new ArgumentException("The email address is not valid, no '@' found");
        }
        if (!value.Contains('.'))
        {
            throw new ArgumentException("The email address is not valid, no '.' found");
        }
        return new EmailAddress(value);
    }

    public static implicit operator string(EmailAddress x) => x.Value;
    
    public string Value { get; }

    public bool Matches(EmailAddress emailAddress) =>
        string.Equals(emailAddress.Value, Value, StringComparison.InvariantCultureIgnoreCase);

    public override string ToString() => $"{Value}";
}