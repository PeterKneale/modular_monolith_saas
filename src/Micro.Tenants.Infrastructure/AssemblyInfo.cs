using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Micro.Tenants.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]

namespace Micro.Tenants.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}