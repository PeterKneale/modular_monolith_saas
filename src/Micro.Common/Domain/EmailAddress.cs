namespace Micro.Common.Domain;

public class EmailAddress : ValueObject
{
    private EmailAddress(string canonical, string display)
    {
        Canonical = canonical;
        Display = display;
    }

    public string Display { get; }
    public string Canonical { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email address cannot be empty");

        if (!value.Contains('@')) throw new ArgumentException("The email address is not valid, no '@' found");

        if (!value.Contains('.')) throw new ArgumentException("The email address is not valid, no '.' found");

        var display = value;
        var canonical = value.ToLowerInvariant();
        return new EmailAddress(canonical, display);
    }

    public static implicit operator string(EmailAddress x) => x.Display;

    public bool Matches(EmailAddress emailAddress) =>
        string.Equals(emailAddress.Canonical, Canonical);

    public override string ToString() => $"{Display}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Canonical;
    }
}