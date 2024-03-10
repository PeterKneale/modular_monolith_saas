using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class NameTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Organisation_name_can_be_changed()
    {
        // arrange
        var userId = await RegisterUser();
        var organisationId = await CreateOrganisation(userId);
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
        var userId = await RegisterUser();
        var name = Guid.NewGuid().ToString()[..10];
        
        // act
        await CreateOrganisation(userId, name);
        var action = async () =>  await CreateOrganisation(userId, name);;

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>($"*{name}*");
    }

    [Fact]
    public async Task Cant_update_organisation_name_if_used()
    {
        // arrange
        var userId = await RegisterUser();
        var name = Guid.NewGuid().ToString()[..10];
        await CreateOrganisation(userId, name);
        
        // act
        var organisationId = await CreateOrganisation(userId);
        var action = async () => await UpdateOrganisationName(name, userId, organisationId);

        // assert
        await action.Should().ThrowAsync<AlreadyInUseException>();
    }
}