namespace Micro.Web.Code.Contexts.Page;

public interface IPageContextProject
{
    Guid Id { get; }
    string Name { get; }
}

public class PageContextProject(Guid id, string name) : IPageContextProject
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}