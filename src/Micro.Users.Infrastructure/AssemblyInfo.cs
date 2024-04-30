using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: ExcludeFromCodeCoverage]
[assembly: InternalsVisibleTo("Micro.Users.Domain.UnitTests")]
[assembly: InternalsVisibleTo("Micro.Users.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]
public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}