using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Projects.Commands;
using Micro.Tenants.Application.Projects.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Projects;

[Collection(nameof(ServiceFixtureCollection))]
public class ProjectTests(ServiceFixture service, ITestOutputHelper outputHelper)  :BaseTest(service, outputHelper)
{
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
        await Service.Command(register);
        await Service.Command(new CreateOrganisation.Command(organisationId, orgName), userId);
        await Service.Command(new CreateProject.Command(projectId, projectName), userId, organisationId);
        
        // Assert can get by id
        var getById = new GetProjectById.Query(projectId);
        var getByIdResult = await Service.Query(getById);
        getByIdResult.Id.Should().Be(projectId);
        getByIdResult.Name.Should().Be(projectName);
        
        // Assert can get by name
        var getByName = new GetProjectByName.Query(projectName);
        var getByNameResult = await Service.Query(getByName);
        getByNameResult.Id.Should().Be(projectId);
        getByNameResult.Name.Should().Be(projectName);

        // assert can get by context
        var getByContext = new GetProjectByContext.Query();
        var getByContextResult = await Service.Query(getByContext, userId, organisationId, projectId);
        getByContextResult.Id.Should().Be(projectId);
        getByContextResult.Name.Should().Be(projectName);
        
        await Service.Command(new UpdateProjectName.Command(projectNameUpdated), userId, organisationId, projectId);
        
        // Assert can get by name
        var getByUpdatedName = new GetProjectByName.Query(projectNameUpdated);
        var getByUpdatedNameResult = await Service.Query(getByUpdatedName);
        getByUpdatedNameResult.Id.Should().Be(projectId);
        getByUpdatedNameResult.Name.Should().Be(projectNameUpdated);
    }
}