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
        var userId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();

        // act
        await CreateUser(userId);
        await CreateOrganisation(userId, organisationId);

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
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var organisationId = Guid.NewGuid();

        // act
        await CreateUser(userId1);
        await CreateUser(userId2);
        await CreateOrganisation(userId1, organisationId);
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
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        
        // act
        await CreateUser(userId1);
        await CreateUser(userId2);
        await CreateOrganisation(userId1, organisationId);
        await CreateMember(userId2, userId1, organisationId);
        await DeleteMember(userId2, userId1, organisationId);

        // assert
        var members = await Service.Query(new ListMemberships.Query(), userId2);
        members.Should().BeEmpty();
    }

}