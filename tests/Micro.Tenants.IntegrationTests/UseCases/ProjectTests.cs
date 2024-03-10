using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Projects.Commands;
using Micro.Tenants.Application.Projects.Queries;

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
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var orgName = Guid.NewGuid().ToString()[..10];
        var projectName = Guid.NewGuid().ToString()[..10];        
        var projectNameUpdated = Guid.NewGuid().ToString()[..10];

        // act
        await _service.Command(register);
        await _service.Command(new CreateOrganisation.Command(organisationId, orgName), userId);
        await _service.Command(new CreateProject.Command(projectId, projectName), userId, organisationId);
        
        // Assert can get by id
        var getById = new GetProjectById.Query(projectId);
        var getByIdResult = await _service.Query(getById);
        getByIdResult.Id.Should().Be(projectId);
        getByIdResult.Name.Should().Be(projectName);
        
        // Assert can get by name
        var getByName = new GetProjectByName.Query(projectName);
        var getByNameResult = await _service.Query(getByName);
        getByNameResult.Id.Should().Be(projectId);
        getByNameResult.Name.Should().Be(projectName);

        // assert can get by context
        var getByContext = new GetProjectByContext.Query();
        var getByContextResult = await _service.Query(getByContext, userId, organisationId, projectId);
        getByContextResult.Id.Should().Be(projectId);
        getByContextResult.Name.Should().Be(projectName);
        
        await _service.Command(new UpdateProjectName.Command(projectNameUpdated), userId, organisationId, projectId);
        
        // Assert can get by name
        var getByUpdatedName = new GetProjectByName.Query(projectNameUpdated);
        var getByUpdatedNameResult = await _service.Query(getByUpdatedName);
        getByUpdatedNameResult.Id.Should().Be(projectId);
        getByUpdatedNameResult.Name.Should().Be(projectNameUpdated);
    }
}