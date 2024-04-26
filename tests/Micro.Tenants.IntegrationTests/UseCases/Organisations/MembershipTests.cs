using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;
using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class MembershipTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Creating_an_organisation_also_creates_owner_membership()
    {
        // arrange
        var userId =  await GivenUser();

        // act
        var organisationId = await GivenOrganisation(userId);

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
        var userId1 =  await GivenUser();
        var userId2 =  await GivenUser();
        var organisationId = await GivenOrganisation(userId1);

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
        var userId1 =  await GivenUser();
        var userId2 =  await GivenUser();
        var organisationId = await GivenOrganisation(userId1);
        
        // act
        await CreateMember(userId2, userId1, organisationId);
        await DeleteMember(userId2, userId1, organisationId);

        // assert
        var members = await Service.Query(new ListMemberships.Query(), userId2);
        members.Should().BeEmpty();
    }


    private async Task CreateMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var createMember = new CreateMember.Command(userId);
        await Service.Command(createMember, ctxUserId, ctxOrganisationId);
    }

    private async Task DeleteMember(Guid userId, Guid ctxUserId, Guid ctxOrganisationId)
    {
        var deleteMember = new RemoveMember.Command(userId);
        await Service.Command(deleteMember, ctxUserId, ctxOrganisationId);
    }
}