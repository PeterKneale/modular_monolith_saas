using Micro.Common.Web.Contexts.PageContext;

namespace Micro.Web.Code.Contexts.PageContext;

public class PageContextProject(Guid id, string name) : IPageContextProject
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}