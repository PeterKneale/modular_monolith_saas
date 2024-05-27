using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Micro.Users.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]

namespace Micro.Users.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}