using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.Domain.OrganisationAggregate.Rules;

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
        var membership = _memberships.Single(x => x.UserId.Equals(userId));
        _memberships.Remove(membership);
    }

    public void UpdateMembershipRole(UserId userId, MembershipRole role)
    {
        CheckRule(new MembershipMustExistRule(_memberships, userId));
        var membership = _memberships.Single(x => x.UserId.Equals(userId));
        membership.SetRole(role);
    }

    public void AddProject(ProjectId projectId, ProjectName name)
    {
        CheckRule(new ProjectMustNotExistRule(_projects, projectId));
        CheckRule(new ProjectNameMustNotExistRule(_projects, name));
        _projects.Add(new Project(projectId, OrganisationId, name));
    }

    public void UpdateProjectName(ProjectId projectId, ProjectName projectName)
    {
        CheckRule(new ProjectMustExistRule(_projects, projectId));
        CheckRule(new ProjectNameMustNotExistRule(_projects, projectName));
        var project = _projects.Single(x => x.ProjectId.Equals(projectId));
        project.ChangeName(projectName);
    }
}