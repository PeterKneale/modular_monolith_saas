using static Micro.Web.Code.Contexts.Page.Constants;

namespace Micro.Web.Code.Contexts.Page;

public static class HttpContextExtensions
{
    public static bool HasOrganisationContext(this HttpContext context) => 
        context.Items.ContainsKey(OrganisationContextKey);
    
    public static bool HasProjectContext(this HttpContext context) => 
        context.Items.ContainsKey(ProjectContextKey);

    public static void SetOrganisationContext(this HttpContext context, PageContextOrganisation value) => 
        context.Items[OrganisationContextKey] = value;
    public static void SetProjectContext(this HttpContext context, PageContextProject value) => 
        context.Items[ProjectContextKey] = value;

    public static PageContextOrganisation? GetOrganisationContext(this HttpContext context) => 
        context.Items[OrganisationContextKey] as PageContextOrganisation;
    
    public static PageContextProject? GetProjectContext(this HttpContext context) => 
        context.Items[ProjectContextKey] as PageContextProject;
}