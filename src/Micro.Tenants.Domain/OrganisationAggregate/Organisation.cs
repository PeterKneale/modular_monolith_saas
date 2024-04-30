using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.Domain.OrganisationAggregate.Rules;
using NotSupportedException = System.NotSupportedException;

namespace Micro.Tenants.Domain.OrganisationAggregate;

public class Organisation : BaseEntity
{
    private readonly List<Membership> _memberships = null!;
    private readonly List<Project> _projects = null!;

    private Organisation()
    {
        // ef core
    }

    private Organisation(OrganisationId organisationId, OrganisationName name, UserId ownerId)
    {
        OrganisationId = organisationId;
        Name = name;
        _memberships = [Membership.CreateOwner(organisationId, ownerId)];
        _projects = new();
        AddDomainEvent(new OrganisationCreatedDomainEvent(organisationId, name));
    }

    public OrganisationId OrganisationId { get; } = null!;

    public OrganisationName Name { get; private set; } = null!;

    public IReadOnlyCollection<Membership> Memberships => _memberships;
    public IReadOnlyCollection<Project> Projects => _projects;

    public static Organisation Create(OrganisationId id, OrganisationName name, UserId ownerId) =>
        new(id, name, ownerId);

    public void ChangeName(OrganisationName name)
    {
        AddDomainEvent(new OrganisationNameChangedDomainEvent(OrganisationId, name));
        Name = name;
    }

    public void AddMember(UserId userId)
    {
        CheckRule(new MembershipMustNotExistRule(_memberships, userId));
        _memberships.Add(Membership.CreateMember(OrganisationId, userId));
    }

    public void RemoveMember(UserId userId)
    {
        CheckRule(new MembershipMustExistRule(_memberships, userId));
        CheckRule(new MembershipMustBeForRole(_memberships, userId, MembershipRole.Member));
        var membership = _memberships.Single(x => x.UserId.Equals(userId));
        _memberships.Remove(membership);
    }

    public void PromoteMemberToOwner(UserId userId)
    {
        CheckRule(new MembershipMustExistRule(_memberships, userId));
        CheckRule(new MembershipMustBeForRole(_memberships, userId, MembershipRole.Member));
        var membership = _memberships.Single(x => x.UserId.Equals(userId));
        membership.SetRole(MembershipRole.Owner);
    }

    public void DemoteOwnerToMember(UserId userId)
    {
        CheckRule(new MembershipMustExistRule(_memberships, userId));
        CheckRule(new MembershipMustBeForRole(_memberships, userId, MembershipRole.Owner));
        CheckRule(new OtherOwnersMustExistRule(_memberships, userId));
        var membership = _memberships.Single(x => x.UserId.Equals(userId));
        membership.SetRole(MembershipRole.Member);
    }

    public void AddProject(ProjectId projectId, ProjectName name)
    {
        CheckRule(new ProjectMustNotExistRule(_projects, projectId));
        CheckRule(new ProjectNameMustNotExistRule(_projects, name));
        _projects.Add(new Project(projectId, OrganisationId, name));
        AddDomainEvent(new ProjectCreatedDomainEvent(OrganisationId, projectId, name));
    }

    public void UpdateProjectName(ProjectId projectId, ProjectName projectName)
    {
        CheckRule(new ProjectMustExistRule(_projects, projectId));
        CheckRule(new ProjectNameMustNotExistRule(_projects, projectName));
        var project = _projects.Single(x => x.ProjectId.Equals(projectId));
        project.ChangeName(projectName);
        AddDomainEvent(new ProjectUpdatedDomainEvent(projectId, projectName));
    }
}

public class MembershipMustBeForRole(List<Membership> memberships, UserId userId, MembershipRole role) : IBusinessRule
{
    public string Message => $"Membership must be for role {role.Name}";

    public bool IsBroken()
    {
        foreach (var membership in memberships)
        {
            // find membership for user
            if (membership.UserId.Equals(userId))
            {
                // the rule is broken if the role is not the same
                return !membership.Role.Equals(role);
            }
        }

        throw new NotSupportedException("No membership is present");
    }
}