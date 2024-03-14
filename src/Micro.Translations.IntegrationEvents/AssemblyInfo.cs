using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: ExcludeFromCodeCoverage]

namespace Micro.Translations.IntegrationEvents;

public static class IntegrationEventsAssemblyInfo
{
    public static Assembly Assembly => typeof(IntegrationEventsAssemblyInfo).Assembly;
}