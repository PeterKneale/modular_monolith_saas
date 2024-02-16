namespace Micro.Web.Code.PageContext;

public interface IOrganisationPageContext
{
    Guid Id { get; }
    string Name { get; }
}

public class OrganisationPageContext(Guid id, string name) : IOrganisationPageContext
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}