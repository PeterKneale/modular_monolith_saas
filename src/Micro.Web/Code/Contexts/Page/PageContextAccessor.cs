namespace Micro.Web.Code.Contexts.Page;

internal class PageContextAccessor(IHttpContextAccessor accessor) : IPageContextAccessor
{
    public bool HasOrganisation => accessor.HttpContext!.GetOrganisationContext() != null;

    public bool HasProject => accessor.HttpContext!.GetProjectContext() != null;

    public IPageContextOrganisation Organisation =>
        accessor.HttpContext!.GetOrganisationContext() ??
        throw new InvalidOperationException("Organisation context not found");

    public IPageContextProject Project =>
        accessor.HttpContext!.GetProjectContext() ??
        throw new InvalidOperationException("Project context not found");
}