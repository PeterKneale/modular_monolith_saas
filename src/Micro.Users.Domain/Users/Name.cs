namespace Micro.Users.Domain.Users;

public class Name : ValueObject
{
    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }

    public static Name Create(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first))
        {
            throw new ArgumentException("First name cannot be empty");
        }
        if (string.IsNullOrWhiteSpace(last))
        {
            throw new ArgumentException("Last name cannot be empty");
        }
        return new Name(first, last);
    }

    private string FullName => $"{First} {Last}";

    public string First { get; }

    public string Last { get; }

    public static implicit operator string(Name x) => x.FullName;

    public override string ToString() => FullName;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }
}