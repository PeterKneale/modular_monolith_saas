using System.Security.Claims;

namespace Micro.Web.Code.Contexts.Authentication;

public static class HttpContextExtensions
{
    public static bool IsAuthenticated(this HttpContext context)
    {
        var identity = context.User.Identity;
        return identity is not null && identity.IsAuthenticated;
    }
    
    public static Guid GetUserId(this HttpContext context)
    {
        var claim = GetClaim(context, Constants.UserClaimUserId);
        return claim != null 
            ? Guid.Parse(claim.Value) 
            : throw new InvalidOperationException("User Id claim not found");
    }
    
    public static string GetUserEmail(this HttpContext context)
    {
        var claim = GetClaim(context, Constants.UserClaimEmail);
        return claim != null 
            ? claim.Value
            : throw new InvalidOperationException("User Email claim not found");
    }

    private static Claim? GetClaim(HttpContext context, string key)
    {
        return context.User.FindFirst(key);
    }
}