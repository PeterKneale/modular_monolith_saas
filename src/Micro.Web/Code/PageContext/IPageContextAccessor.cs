namespace Micro.Web.Code.PageContext;

public interface IPageContextAccessor
{
    IOrganisationPageContext? Organisation { get; }
    IProjectPageContext? Project { get; }
}