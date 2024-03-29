namespace Micro.Users.Infrastructure;

internal static class CompositionRoot
{
    private static IServiceProvider? _provider;

    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static IServiceScope BeginLifetimeScope() =>
        _provider?.CreateScope() ?? throw new Exception("Service provider not set.");
}
