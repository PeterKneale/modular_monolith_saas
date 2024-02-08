namespace Micro.Tenants.Domain.Users;

public class UserRole(string name)
{
    public string Name { get; init; } = name;
    
    public static UserRole Admin => new UserRole("Admin");
}