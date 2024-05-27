using static Micro.Common.Web.Contexts.PageContext.Constants;

namespace Micro.Common.Web.Contexts.PageContext;

public static class PageContextExtensions
{
    public static void SetOrganisationContext(this HttpContext context, IPageContextOrganisation value) =>
        context.Items[OrganisationContextKey] = value;

    public static void SetProjectContext(this HttpContext context, IPageContextProject value) =>
        context.Items[ProjectContextKey] = value;

    public static IPageContextOrganisation? GetOrganisationContext(this HttpContext context) =>
        context.Items[OrganisationContextKey] as IPageContextOrganisation;

    public static IPageContextProject? GetProjectContext(this HttpContext context) =>
        context.Items[ProjectContextKey] as IPageContextProject;
}