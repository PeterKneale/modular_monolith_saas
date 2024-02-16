namespace Micro.Web.Code.PageContext;

public interface IProjectPageContext
{
    Guid Id { get; }
    string Name { get; }
}

public class ProjectPageContext(Guid id, string name) : IProjectPageContext
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}