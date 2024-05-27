namespace Micro.Users.Domain.Users;

public record Password
{
    private Password(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Password Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Password cannot be empty");
        return new Password(value);
    }

    public static implicit operator string(Password x) => x.Value;

    public override string ToString() => "*******";
}