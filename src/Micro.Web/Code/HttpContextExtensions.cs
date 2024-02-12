namespace Micro.Web.Code;

public static class HttpContextExtensions
{
    public static bool HasOrganisationId(this HttpContext context) => context.Items.ContainsKey(Constants.OrgHttpItemKey);

    public static void SetOrganisationId(this HttpContext context, Guid organisationId) => context.Items[Constants.OrgHttpItemKey] = organisationId;

    public static Guid? GetOrganisationId(this HttpContext context)
    {
        var item = context.Items[Constants.OrgHttpItemKey];
        return item == null ? null : Guid.Parse(item.ToString()!);
    }
    
    public static bool HasAppId(this HttpContext context) => context.Items.ContainsKey(Constants.AppHttpItemKey);

    public static void SetAppId(this HttpContext context, Guid appId) => context.Items[Constants.AppHttpItemKey] = appId;

    public static Guid? GetProjectId(this HttpContext context)
    {
        var item = context.Items[Constants.AppHttpItemKey];
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