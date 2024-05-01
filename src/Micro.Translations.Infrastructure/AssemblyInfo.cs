using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Micro.Translations.IntegrationTests")]
[assembly: InternalsVisibleTo("Micro.Modules.IntegrationTests")]

namespace Micro.Translations.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}