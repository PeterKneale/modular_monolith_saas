namespace Micro.Common.Web.Contexts.PageContext;

public interface IPageContextAccessor
{
    bool HasOrganisation { get; }
    bool HasProject { get; }
    IPageContextOrganisation Organisation { get; }
    IPageContextProject Project { get; }
}