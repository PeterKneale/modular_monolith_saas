namespace Micro.Tenants.Domain.OrganisationAggregate;

public class MembershipRole
{
    private MembershipRole(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static MembershipRole Owner => new("Owner");
    public static MembershipRole Member => new("Member");

    public static MembershipRole FromString(string name) => name switch
    {
        nameof(Owner) => Owner,
        nameof(Member) => Member,
        _ => throw new ArgumentException($"Invalid role name: {name}")
    };
}