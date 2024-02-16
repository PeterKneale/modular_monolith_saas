using Micro.Web.Code.PageContext;
using static Micro.Web.Code.Contexts.Constants;

namespace Micro.Web.Code.Contexts;

public static class HttpContextExtensions
{
    public static bool HasOrganisationContext(this HttpContext context) => 
        context.Items.ContainsKey(OrganisationContextKey);
    
    public static bool HasProjectContext(this HttpContext context) => 
        context.Items.ContainsKey(ProjectContextKey);

    public static void SetOrganisationContext(this HttpContext context, OrganisationPageContext value) => 
        context.Items[OrganisationContextKey] = value;
    public static void SetProjectContext(this HttpContext context, ProjectPageContext value) => 
        context.Items[ProjectContextKey] = value;

    public static OrganisationPageContext? GetOrganisationContext(this HttpContext context) => 
        context.Items[OrganisationContextKey] as OrganisationPageContext;
    
    public static ProjectPageContext? GetProjectContext(this HttpContext context) => 
        context.Items[ProjectContextKey] as ProjectPageContext;

    public static Guid? GetUserId(this HttpContext context)
    {
        var identity = context.User.Identity;
        if (identity is null)
        {
            return null;
        }

        var authenticated = identity.IsAuthenticated;
        if (!authenticated)
        {
            return null;
        }

        var claim = context.User.FindFirst(UserClaimKey);
        if (claim == null)
            return null;

        return Guid.Parse(claim.Value);
    }
}