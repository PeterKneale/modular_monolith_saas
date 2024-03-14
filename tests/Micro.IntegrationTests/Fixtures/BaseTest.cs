namespace Micro.IntegrationTests.Fixtures;

public class BaseTest
{
    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        Service = service;
    }

    protected ServiceFixture Service { get; }
}