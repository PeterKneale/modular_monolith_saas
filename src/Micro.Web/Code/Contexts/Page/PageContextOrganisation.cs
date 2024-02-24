namespace Micro.Web.Code.Contexts.Page;

public class PageContextOrganisation(Guid id, string name) : IPageContextOrganisation
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}