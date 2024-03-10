using Micro.Tenants.Application.Memberships.Queries;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.IntegrationTests.UseCases.Memberships;

[Collection(nameof(ServiceFixtureCollection))]
public class MembershipTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Creating_an_organisation_also_creates_membership()
    {
        // arrange
        var userId = await RegisterUser();

        // act
        var organisationId = await CreateOrganisation(userId);

        // assert
        var memberships = await Service.Query(new ListMemberships.Query(), userId);
        memberships.Should().ContainSingle(x =>
            x.OrganisationId == organisationId &&
            x.RoleName == MembershipRole.Owner.Name);
    }

    [Fact]
    public async Task Member_can_be_created()
    {
        // arrange
        var userId1 = await RegisterUser();
        var userId2 = await RegisterUser();
        var organisationId = await CreateOrganisation(userId1);

        // act
        await CreateMember(userId2, userId1, organisationId);

        // assert
        var members = await Service.Query(new ListMemberships.Query(), userId2);
        members.Should().ContainSingle(x =>
            x.OrganisationId == organisationId &&
            x.RoleName == MembershipRole.Member.Name);
    }

    [Fact]
    public async Task Member_can_be_deleted()
    {
        // arrange
        var userId1 = await RegisterUser();
        var userId2 = await RegisterUser();
        var organisationId = await CreateOrganisation(userId1);
        await CreateMember(userId2, userId1, organisationId);
        
        // act
        await DeleteMember(userId2, userId1, organisationId);

        // assert
        var members = await Service.Query(new ListMemberships.Query(), userId2);
        members.Should().BeEmpty();
    }

}