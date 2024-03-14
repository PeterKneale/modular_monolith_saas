using System.Reflection;

[assembly: ExcludeFromCodeCoverage]

namespace Micro.Common;

public static class CommonAssemblyInfo
{
    public static Assembly Assembly => typeof(CommonAssemblyInfo).Assembly;
}