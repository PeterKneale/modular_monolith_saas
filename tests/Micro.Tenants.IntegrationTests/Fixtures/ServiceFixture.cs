using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Domain;
using Micro.Tenants.Application.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        var services = new ServiceCollection()
            .AddLogging(x => x.AddXUnit(this))
            .BuildServiceProvider();
        var logFactory = services.GetRequiredService<ILoggerFactory>();

        _accessor = new TestContextAccessor();
        _module = new TenantsModule();

        TenantsModuleStartup.Start(_accessor, configuration, logFactory, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task Command(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        await _module.SendCommand(command);
        ClearContext();
    }

    public async Task<T> Query<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        var result = await _module.SendQuery(query);
        ClearContext();
        return result;
    }

    private void ClearContext()
    {
        _accessor.User = null;
        _accessor.Organisation = null;
        _accessor.Project = null;
    }

    private void SetProjectContext(Guid? projectId)
    {
        if (!projectId.HasValue) return;
        OutputHelper?.WriteLine($"Setting project ID to {projectId}");
        _accessor.Project = new ProjectExecutionContext(new ProjectId(projectId.Value));
    }

    private void SetOrgContext(Guid? organisationId)
    {
        if (!organisationId.HasValue) return;
        OutputHelper?.WriteLine($"Setting organisation ID to {organisationId}");
        _accessor.Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId.Value));
    }

    private void SetUserContext(Guid? userId)
    {
        if (!userId.HasValue) return;
        OutputHelper?.WriteLine($"Setting user ID to {userId}");
        _accessor.User = new UserExecutionContext(new UserId(userId.Value));
    }
}