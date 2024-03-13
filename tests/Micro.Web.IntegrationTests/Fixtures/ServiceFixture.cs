using MartinCostello.Logging.XUnit;
using MediatR;
using Micro.Common;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Tenants;
using Micro.Translations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Micro.Web.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor
{
    private readonly IModule _tenants;
    private readonly IModule _translations;
    private readonly TestContextAccessor _accessor;

    public ServiceFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
        
        var services = new ServiceCollection()
            .AddLogging(x => x.AddXUnit(this))
            .AddInMemoryEventBus()
            .BuildServiceProvider();
        
        var bus = services.GetRequiredService<IEventsBus>();
        var logs = services.GetRequiredService<ILoggerFactory>();

        _accessor = new TestContextAccessor();
        _tenants = new TenantsModule();
        _translations = new TranslationModule();

        TenantsModuleStartup.Start(_accessor, configuration, bus, logs, true);
        TranslationModuleStartup.Start(_accessor, configuration, bus, logs, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task ExecuteTenants(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        await action(_tenants);
        ClearContext();
    }
    public async Task ExecuteTranslations(Func<IModule, Task> action, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        await action(_tenants);
        ClearContext();
    }

    public async Task CommandTenants(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        await _tenants.SendCommand(command);
        ClearContext();
    }
    public async Task CommandTranslations(IRequest command, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        await _translations.SendCommand(command);
        ClearContext();
    }
    
    public async Task PublishTenants(IntegrationEvent integrationEvent)
    {
        await _tenants.PublishNotification(integrationEvent);
    }
    
    public async Task PublishTranslations(IntegrationEvent integrationEvent)
    {
        await _translations.PublishNotification(integrationEvent);
    }

    public async Task<T> QueryTenants<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        var result = await _tenants.SendQuery(query);
        ClearContext();
        return result;
    }
    public async Task<T> QueryTranslations<T>(IRequest<T> query, Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        SetUserContext(userId);
        SetOrgContext(organisationId);
        SetProjectContext(projectId);
        var result = await _translations.SendQuery(query);
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