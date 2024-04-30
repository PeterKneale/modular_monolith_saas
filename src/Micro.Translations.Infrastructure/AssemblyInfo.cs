using System.Reflection;

[assembly: ExcludeFromCodeCoverage]

namespace Micro.Translations;

public static class InfrastructureAssemblyInfo
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyInfo).Assembly;
}