using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: ExcludeFromCodeCoverage]
[assembly: InternalsVisibleTo("Micro.Users.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]
namespace Micro.Users.Infrastructure;

public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}