using System.Reflection;

[assembly: ExcludeFromCodeCoverage]

namespace Micro.Translations.Infrastructure;

public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}