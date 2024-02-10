using MartinCostello.Logging.XUnit;
using Micro.Common;
using Micro.Common.Domain;

namespace Micro.Translations.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor
{
    private readonly IModule _module;
    private readonly TestContextAccessor _accessor;

    public ServiceFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        _accessor = new TestContextAccessor();
        _module = new TranslationModule();

        TranslationModuleStartup.Start(_accessor, configuration, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public IModule Tenants => _module;

    public async Task Exec(Func<IModule, Task> action)
    {
        await action(_module);
        ClearContext();
    }
    
    public async Task Exec(Func<IModule, Task> action, Guid userId)
    {
        _accessor.User = new UserContext(new UserId(userId));
        await action(_module);
        ClearContext();
    }
    
    public async Task Exec(Func<IModule, Task> action, Guid userId, Guid organisationId)
    {
        _accessor.User = new UserContext(new UserId(userId));
        _accessor.Organisation = new OrganisationContext(new OrganisationId(organisationId));
        await Exec(action, userId);
        ClearContext();
    }
    
    private void ClearContext()
    {
        _accessor.User = null;
        _accessor.Organisation = null;
    }
}