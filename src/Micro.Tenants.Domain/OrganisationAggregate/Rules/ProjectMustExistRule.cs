namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class ProjectMustExistRule(IEnumerable<Project> projects, ProjectId id) : IBusinessRule
{
    public string Message => $"Organisation does not have project with id '{id}'";

    public bool IsBroken() => !projects.Any(x => x.ProjectId.Equals(id));
}