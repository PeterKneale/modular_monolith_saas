namespace Micro.Tenants.IntegrationTests.Fixtures;

public class TestContextAccessor : IContextAccessor
{
    public ICurrentContext? CurrentContext { get; set; }
}