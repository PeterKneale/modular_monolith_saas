using Micro.Common.Web.Contexts.PageContext;

namespace Micro.Web.Code.Contexts.PageContext;

public class PageContextOrganisation(Guid id, string name) : IPageContextOrganisation
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}