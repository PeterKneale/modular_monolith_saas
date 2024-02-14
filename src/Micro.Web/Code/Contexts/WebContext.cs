using Microsoft.AspNetCore.Mvc.Razor;

namespace Micro.Web.Code.Contexts;

internal class WebContext(IHttpContextAccessor accessor) : IWebContext
{
    public bool IsAuthenticated => accessor.HttpContext!.User.Identity!.IsAuthenticated;
    
    public Guid? UserId => accessor.HttpContext!.GetUserId();
    
    public Guid? OrganisationId => accessor.HttpContext!.GetOrganisationId();
    
    public string OrganisationName => accessor.HttpContext!.GetOrganisationName()!;
    
    public Guid? ProjectId => accessor.HttpContext!.GetProjectId();
}

public static class WebContextExtensions
{
    public static IWebContext Ctx(this RazorPage page) => 
        page.Context.RequestServices.GetRequiredService<IWebContext>();

    public static IWebContext Ctx(this PageModel page) =>
        page.HttpContext.RequestServices.GetRequiredService<IWebContext>();
}