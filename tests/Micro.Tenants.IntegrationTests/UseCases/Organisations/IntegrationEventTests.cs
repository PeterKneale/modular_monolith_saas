using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class IntegrationEventTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Creating_an_organisation_publishes_an_integration_events()
    {
        // arrange
        var userId =  await GivenUser();

        // act
        var organisationId = await GivenOrganisation(userId);

        // assert
        (await GetOutboxMessages<OrganisationCreated>())
            .Should()
            .ContainSingle(x => x.OrganisationId == organisationId);
    }

    [Fact]
    public async Task Creating_and_updating_project_publishes_an_integration_events()
    {
        // arrange
        var userId =  await GivenUser();
        var organisationId = await GivenOrganisation(userId);
        var projectId = Guid.NewGuid();
        var projectName = Guid.NewGuid().ToString()[..10];
        var projectNameUpdated = Guid.NewGuid().ToString()[..10];

        // act
        await Service.Command(new CreateProject.Command(projectId, projectName), userId, organisationId);
        await Service.Command(new UpdateProjectName.Command(projectNameUpdated), userId, organisationId, projectId);

        // assert
        (await GetOutboxMessages<ProjectCreated>())
            .Should()
            .ContainSingle(x => x.ProjectId == projectId);
        (await GetOutboxMessages<ProjectUpdated>())
            .Should()
            .ContainSingle(x => x.ProjectId == projectId);
    }
}