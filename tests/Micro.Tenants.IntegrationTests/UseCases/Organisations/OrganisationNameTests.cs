using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class OrganisationNameTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Organisation_name_can_be_changed()
    {
        // arrange
        var userId = await GivenUser();
        var organisationId = await GivenOrganisation(userId);
        var name = Guid.NewGuid().ToString()[..10];

        // act
        var command = new UpdateOrganisationName.Command(name);
        await Service.Command(command, userId, organisationId);

        // assert
        var organisation = await Service.Query(new GetOrganisationByName.Query(name));
        organisation.Id.Should().Be(organisationId);
    }

    [Fact]
    public async Task Cant_create_organisation_if_name_used()
    {
        // arrange
        var userId = await GivenUser();
        var organisationName = Guid.NewGuid().ToString()[..10];
        var organisationId1 = await GivenOrganisation(userId, organisationName);

        // act
        var action = async () =>
        {
            var organisationId2 = Guid.NewGuid();
            var create = new CreateOrganisation.Command(organisationId2, organisationName);
            await Service.Command(create, userId);
        };

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>($"*{organisationName}*");
    }

    [Fact]
    public async Task Cant_update_organisation_name_if_used()
    {
        // arrange
        var userId = await GivenUser();
        var organisationId1 = await GivenOrganisation(userId, "X");
        var organisationId2 = await GivenOrganisation(userId, "Y");

        // act
        var action = async () =>
        {
            var update = new UpdateOrganisationName.Command("X");
            await Service.Command(update, userId, organisationId2);
        };

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>();
    }
}