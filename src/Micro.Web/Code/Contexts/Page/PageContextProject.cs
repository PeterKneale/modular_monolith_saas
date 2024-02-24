namespace Micro.Web.Code.Contexts.Page;

public class PageContextProject(Guid id, string name) : IPageContextProject
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}