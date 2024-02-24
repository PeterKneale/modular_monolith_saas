namespace Micro.Web.Code.Contexts.Page;

public interface IPageContextAccessor
{
    bool HasOrganisation { get; }
    bool HasProject { get; }
    IPageContextOrganisation Organisation { get; }
    IPageContextProject Project { get; }
}