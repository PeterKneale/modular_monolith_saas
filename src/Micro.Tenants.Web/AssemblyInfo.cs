using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Micro.Tenants.Web;

[ExcludeFromCodeCoverage]
public static class TenantsWebAssemblyInfo
{
    public static Assembly Assembly => typeof(TenantsWebAssemblyInfo).Assembly;
}