namespace Micro.Tenants.Domain.Users;

public class UserName(string first, string last)
{
    public string First { get; init; } = first;
    public string Last { get; init; } = last;
}