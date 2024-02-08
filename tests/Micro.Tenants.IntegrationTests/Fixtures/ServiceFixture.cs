using MartinCostello.Logging.XUnit;
using Micro.Common;
using Micro.Common.Domain;

namespace Micro.Tenants.IntegrationTests.Fixtures;

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
        _module = new TenantsModule();

        TenantsModuleStartup.Start(_accessor, configuration, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public IModule Tenants => _module;

    public async Task Exec(Func<IModule, Task> action, Guid organisationId, Guid userId)
    {
        SetContext(organisationId, userId);
        await action(_module);
        ClearContext();
    }
    
    public async Task Exec(Func<IModule, Task> action)
    {
        ClearContext();
        await action(_module);
    }
    
    private void SetContext(Guid organisationId, Guid userId) => 
        _accessor.CurrentContext = new CurrentContext(new OrganisationId(organisationId), new UserId(userId));

    private void ClearContext() => 
        _accessor.CurrentContext = null;

}