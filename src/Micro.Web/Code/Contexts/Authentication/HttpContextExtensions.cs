namespace Micro.Web.Code.Contexts.Authentication;

public static class HttpContextExtensions
{
    public static bool IsAuthenticated(this HttpContext context)
    {
        var identity = context.User.Identity;
        return identity is not null && identity.IsAuthenticated;
    }

    public static bool HasUserId(this HttpContext context)
    {
        return context.Items.ContainsKey(Constants.UserIdKey);
    }
    public static void SetUserId(this HttpContext context, Guid userId)
    {
        context.Items.Add(Constants.UserIdKey, userId.ToString());
    }

    public static void SetUserEmail(this HttpContext context, string email)
    {
        context.Items.Add(Constants.UserEmailKey, email);
    }

    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(Constants.UserIdKey);
        if (claim != null)
        {
            return Guid.Parse(claim.Value);
        }

        if (context.Items.TryGetValue(Constants.UserIdKey, out var value))
        {
            return Guid.Parse(value!.ToString()!);
        }

        throw new InvalidOperationException("User Id not found");
    }

    public static string GetUserEmail(this HttpContext context)
    {
        var claim = context.User.FindFirst(Constants.UserEmailKey);
        if (claim != null)
        {
            return claim.Value;
        }
        if (context.Items.TryGetValue(Constants.UserEmailKey, out var value))
        {
            return value!.ToString()!;
        }
        
        throw new InvalidOperationException("User Email not found");
    }
}