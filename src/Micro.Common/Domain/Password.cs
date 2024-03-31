namespace Micro.Common.Domain;

public record Password
{
    private Password(string value)
    {
        Value = value;
    }

    public static Password Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Password cannot be empty");
        }
        return new Password(value);
    }

    public static implicit operator string(Password x) => x.Value;
    
    public string Value { get; }

    public bool Matches(Password password) =>
        password.Value == Value;

    public override string ToString() => "*******";
}