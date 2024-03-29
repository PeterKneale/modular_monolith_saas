namespace Micro.Users.Domain.Users;

public class UserName(string first, string last)
{
    public string FullName => $"{First} {Last}";

    public string First { get; init; } = first;

    public string Last { get; init; } = last;

    public static implicit operator string(UserName x) => x.FullName;

    public override string ToString() => FullName;
}