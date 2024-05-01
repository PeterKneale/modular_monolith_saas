using System.Reflection;

namespace Micro.Common;

[ExcludeFromCodeCoverage]
public static class CommonAssemblyInfo
{
    public static Assembly Assembly => typeof(CommonAssemblyInfo).Assembly;
}