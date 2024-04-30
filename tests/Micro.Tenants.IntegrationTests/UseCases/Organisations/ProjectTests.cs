using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class ProjectTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_manage_project()
    {
        // arrange
        var userId =  await GivenUser();
        var organisationId = await GivenOrganisation(userId);
        var projectId = Guid.NewGuid();
        var orgName = Guid.NewGuid().ToString()[..10];
        var projectName = Guid.NewGuid().ToString()[..10];
        var projectNameUpdated = Guid.NewGuid().ToString()[..10];

        // act
        await Service.Execute(async module =>
        {
            await module.SendCommand(new CreateProject.Command(projectId, projectName));
            
            // Assert can get by id
            var getById = new GetProjectById.Query(projectId);
            var getByIdResult = await module.SendQuery(getById);
            getByIdResult.Id.Should().Be(projectId);
            getByIdResult.Name.Should().Be(projectName);

        }, userId, organisationId);

        // assert can get by context
        await Service.Execute(async module =>
        {
            // Assert can get by name
            var getByName = new GetProjectByName.Query(projectName);
            var getByNameResult = await module.SendQuery(getByName);
            getByNameResult.Id.Should().Be(projectId);
            getByNameResult.Name.Should().Be(projectName);
            
            // Update project name
            await module.SendCommand(new UpdateProjectName.Command(projectNameUpdated));

            // Assert can get by updated name
            var getByUpdatedName = new GetProjectByName.Query(projectNameUpdated);
            var getByUpdatedNameResult = await module.SendQuery(getByUpdatedName);
            getByUpdatedNameResult.Id.Should().Be(projectId);
            getByUpdatedNameResult.Name.Should().Be(projectNameUpdated);
            
            var getByContext = new GetProjectByContext.Query();
            var getByContextResult = await module.SendQuery(getByContext);
            getByContextResult.Id.Should().Be(projectId);
            getByContextResult.Name.Should().Be(projectNameUpdated);
        }, userId, organisationId, projectId);
    }
}