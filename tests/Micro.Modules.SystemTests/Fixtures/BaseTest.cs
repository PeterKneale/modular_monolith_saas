namespace Micro.Modules.SystemTests.Fixtures;

public class BaseTest
{
    protected BaseTest(SystemFixture system, ITestOutputHelper output)
    {
        system.OutputHelper = output;
        System = system;
    }

    protected SystemFixture System { get; }
}