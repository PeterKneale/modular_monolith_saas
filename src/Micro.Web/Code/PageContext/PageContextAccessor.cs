using Micro.Web.Code.Contexts;

namespace Micro.Web.Code.PageContext;

internal class PageContextAccessor(IHttpContextAccessor accessor) : IPageContextAccessor
{
    public IOrganisationPageContext? Organisation => accessor.HttpContext!.GetOrganisationContext();

    public IProjectPageContext? Project => accessor.HttpContext!.GetProjectContext();
}