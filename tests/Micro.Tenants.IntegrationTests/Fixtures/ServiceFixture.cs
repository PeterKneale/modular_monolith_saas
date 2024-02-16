using MartinCostello.Logging.XUnit;
using Micro.Common;
using Micro.Common.Domain;
using Micro.Tenants.Application.Users;

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
    
    public async Task Exec(Func<IModule, Task> action)
    {
        await action(_module);
        ClearContext();
    }
    
    public async Task Exec(Func<IModule, Task> action, Guid userId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        await action(_module);
        ClearContext();
    }
    public async Task<T> ExecQ<T>(Func<IModule, Task<T>> action, Guid userId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        var t = await action(_module);
        ClearContext();
        return t;
    }

    public async Task<T> ExecQ<T>(Func<IModule, Task<T>> action)
    {
        var t = await action(_module);
        ClearContext();
        return t;
    }
    
    public async Task Exec(Func<IModule, Task> action, Guid userId, Guid organisationId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        _accessor.Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId));
        await Exec(action, userId);
        ClearContext();
    }
    
    private void ClearContext()
    {
        _accessor.User = null;
        _accessor.Organisation = null;
    }
}