namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class ProjectMustNotExistRule(IEnumerable<Project> projects, ProjectId id) : IBusinessRule
{
    public string Message => $"Organisation already has a project with id {id}";

    public bool IsBroken() => projects.Any(x => x.ProjectId.Equals(id));
}