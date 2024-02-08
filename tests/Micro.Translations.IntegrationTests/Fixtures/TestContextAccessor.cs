namespace Micro.Translations.IntegrationTests.Fixtures;

public class TestContextAccessor : IContextAccessor
{
    public ICurrentContext? CurrentContext { get; set; }
}