namespace Micro.Users.Domain.Users;

public class UserName
{
    private UserName(string first, string last)
    {
        First = first;
        Last = last;
    }

    public static UserName Create(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first))
        {
            throw new ArgumentException("First name cannot be empty");
        }
        if (string.IsNullOrWhiteSpace(last))
        {
            throw new ArgumentException("Last name cannot be empty");
        }
        return new UserName(first, last);
    }

    public string FullName => $"{First} {Last}";

    public string First { get; }

    public string Last { get; }

    public static implicit operator string(UserName x) => x.FullName;

    public override string ToString() => FullName;
}