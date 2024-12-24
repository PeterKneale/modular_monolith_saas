namespace Micro.Tenants.Infrastructure;

[ExcludeFromCodeCoverage]
internal static class TenantsCompositionRoot
{
    private static IServiceProvider? _provider;

    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static IServiceScope BeginLifetimeScope() =>
        _provider?.CreateScope() ?? throw new Exception("Service provider not set.");
    
    public static AsyncServiceScope BeginAsyncLifetimeScope() =>
        _provider?.CreateAsyncScope() ?? throw new Exception("Service provider not set.");
}