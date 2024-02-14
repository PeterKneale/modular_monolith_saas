namespace Micro.Web.Code.Contexts;

public static class HttpContextExtensions
{
    public static bool HasOrganisationId(this HttpContext context) => 
        context.Items.ContainsKey(Constants.OrganisationIdHttpItemKey);

    public static void SetOrganisationId(this HttpContext context, Guid organisationId) => 
        context.Items[Constants.OrganisationIdHttpItemKey] = organisationId;

    public static void SetOrganisationName(this HttpContext context, string name) => 
        context.Items[Constants.OrganisationNameHttpItemKey] = name;

    public static Guid? GetOrganisationId(this HttpContext context)
    {
        var item = context.Items[Constants.OrganisationIdHttpItemKey];
        return item == null ? null : Guid.Parse(item.ToString()!);
    }
    
    public static string? GetOrganisationName(this HttpContext context)
    {
        var item = context.Items[Constants.OrganisationNameHttpItemKey];
        return item?.ToString();
    }

    public static bool HasProjectId(this HttpContext context) => 
        context.Items.ContainsKey(Constants.ProjectIdHttpItemKey);

    public static void SetProjectId(this HttpContext context, Guid projectId) => 
        context.Items[Constants.ProjectIdHttpItemKey] = projectId;
    
    public static Guid? GetProjectId(this HttpContext context)
    {
        var item = context.Items[Constants.ProjectIdHttpItemKey];
        return item == null ? null : Guid.Parse(item.ToString()!);
    }

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

        var claim = context.User.FindFirst(Constants.UserClaimKey);
        if (claim == null)
            return null;

        return Guid.Parse(claim.Value);
    }
}