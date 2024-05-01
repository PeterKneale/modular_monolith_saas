using System.Diagnostics.CodeAnalysis;
using System.Reflection;


namespace Micro.Users.Application;

[ExcludeFromCodeCoverage]
public static class ApplicationAssemblyInfo
{
    public static Assembly Assembly => typeof(ApplicationAssemblyInfo).Assembly;
}