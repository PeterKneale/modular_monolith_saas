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
    
    [Obsolete]
    public async Task Exec(Func<IModule, Task> action)
    {
        await action(_module);
        ClearContext();
    }

    [Obsolete]
    public async Task Exec(Func<IModule, Task> action, Guid userId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        await action(_module);
        ClearContext();
    }

    [Obsolete]
    public async Task<T> ExecQ<T>(Func<IModule, Task<T>> action, Guid userId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        var t = await action(_module);
        ClearContext();
        return t;
    }

    [Obsolete]
    public async Task<T> ExecQ<T>(Func<IModule, Task<T>> action)
    {
        var t = await action(_module);
        ClearContext();
        return t;
    }
    
    [Obsolete]
    public async Task Exec(Func<IModule, Task> action, Guid userId, Guid organisationId)
    {
        _accessor.User = new UserExecutionContext(new UserId(userId));
        _accessor.Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId));
        await Exec(action, userId);
        ClearContext();
    }
    
    [Obsolete]
    private void ClearContext()
    {
        _accessor.User = null;
        _accessor.Organisation = null;
    }
    
    public async Task ExecuteInContext(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        if (userId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting user ID to {userId}");
            _accessor.User = new UserExecutionContext(new UserId(userId.Value));
        }

        if (organisationId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting organisation ID to {organisationId}");
            _accessor.Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId.Value));
        }

        if (projectId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting project ID to {projectId}");
            _accessor.Project = new ProjectExecutionContext(new ProjectId(projectId.Value));
        }

        await action(_module);

        _accessor.User = null;
        _accessor.Organisation = null;
        _accessor.Project = null;
    }
    
    public async Task<T> ExecuteInContextQ<T>(Func<IModule, Task<T>> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        if (userId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting user ID to {userId}");
            _accessor.User = new UserExecutionContext(new UserId(userId.Value));
        }

        if (organisationId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting organisation ID to {organisationId}");
            _accessor.Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId.Value));
        }

        if (projectId.HasValue)
        {
            OutputHelper?.WriteLine($"Setting project ID to {projectId}");
            _accessor.Project = new ProjectExecutionContext(new ProjectId(projectId.Value));
        }

        var result = await action(_module);

        _accessor.User = null;
        _accessor.Organisation = null;
        _accessor.Project = null;
        return result;
    }
}