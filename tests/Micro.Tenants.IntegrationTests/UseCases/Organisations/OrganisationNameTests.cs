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
        var userId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var name = Guid.NewGuid().ToString()[..10];

        // act
        await CreateUser(userId);
        await CreateOrganisation(userId, organisationId);
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
        var userId = Guid.NewGuid();
        var organisationId1 = Guid.NewGuid();
        var organisationId2 = Guid.NewGuid();
        var name = Guid.NewGuid().ToString()[..10];

        // act
        await CreateUser(userId);
        await CreateOrganisation(userId, organisationId1, name);
        var action = async () => await CreateOrganisation(userId, organisationId2, name);

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>($"*{name}*");
    }

    [Fact]
    public async Task Cant_update_organisation_name_if_used()
    {
        // arrange
        var userId = Guid.NewGuid();
        var organisationId1= Guid.NewGuid();
        var organisationId2= Guid.NewGuid();
        var name = Guid.NewGuid().ToString()[..10];

        // act
        await CreateUser(userId);
        await CreateOrganisation(userId, organisationId1, name);
        await CreateOrganisation(userId, organisationId2, "X");
        var action = async () => await UpdateOrganisationName(name, userId, organisationId2);

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>();
    }
}