using MartinCostello.Logging.XUnit;
using Micro.Common;
using Micro.Common.Domain;

namespace Micro.Translations.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor
{
    private readonly TestContextAccessor _accessor;

    public ServiceFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        _accessor = new TestContextAccessor();
        Module = new TranslationModule();

        TranslationModuleStartup.Start(_accessor, configuration, true);
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    public IModule Module { get; }

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

        await action(Module);

        _accessor.User = null;
        _accessor.Organisation = null;
        _accessor.Project = null;
    }
}