namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class ProjectNameMustNotExistRule(IEnumerable<Project> projects, ProjectName name) : IBusinessRule
{
    public string Message => $"Organisation already has a project named {name}";

    public bool IsBroken() => projects.Any(x => x.Name.Equals(name));
}