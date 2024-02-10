namespace Micro.Tenants.Domain.Memberships;

public class MembershipRole
{
    public string Name { get; }

    private MembershipRole(string name)
    {
        Name = name;
    }

    public static MembershipRole Owner => new("Owner");
    public static MembershipRole Member => new("Member");

    public static MembershipRole FromString(string name) => name switch
    {
        nameof(Owner) => Owner,
        nameof(Member) => Member,
        _ => throw new ArgumentException($"Invalid role name: {name}")
    };
}