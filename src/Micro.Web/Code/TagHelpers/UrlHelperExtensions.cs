using Constants = Micro.Web.Code.Contexts.Page.Constants;

namespace Micro.Web.Code.TagHelpers;

public static class UrlHelperExtensions
{
    public static string LinkWithPageContext(this IUrlHelper urlHelper, string pageName, object routeValues = null)
    {
        var currentRouteValues = urlHelper.ActionContext.RouteData.Values;
        var newRouteValues = new RouteValueDictionary(routeValues);

        if(currentRouteValues.TryGetValue(Constants.OrganisationRouteKey, out var org))
        {
            newRouteValues[Constants.OrganisationRouteKey] = org;
        }
        if(currentRouteValues.TryGetValue(Constants.ProjectRouteKey, out var project))
        {
            newRouteValues[Constants.ProjectRouteKey] = project;
        }
        
        var link = urlHelper.Page(pageName, newRouteValues);
        if (link == null)
        {
            throw new InvalidOperationException("Link cant be build");
        }
        return link;
    }
}