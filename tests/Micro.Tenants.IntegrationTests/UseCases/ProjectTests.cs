using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Projects.Commands;

namespace Micro.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ProjectTests
{
    private readonly ServiceFixture _service;

    public ProjectTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_project()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var name1 = Guid.NewGuid().ToString()[..10];
        var name2 = Guid.NewGuid().ToString()[..10];
        var org = new CreateOrganisation.Command(organisationId, name1);
        var prj = new CreateProject.Command(organisationId, name2);

        // act
        await _service.Exec(x => x.SendCommand(register));
        await _service.Exec(x => x.SendCommand(org), userId);
        await _service.Exec(x => x.SendCommand(prj), userId, organisationId);

        // assert
    }
}