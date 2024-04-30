using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: ExcludeFromCodeCoverage]
[assembly: InternalsVisibleTo("Micro.Tenants.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]

namespace Micro.Tenants.Infrastructure;

public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}