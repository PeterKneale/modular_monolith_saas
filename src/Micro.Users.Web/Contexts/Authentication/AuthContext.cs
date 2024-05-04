using Micro.Common.Web.Contexts.AuthContext;

namespace Micro.Users.Web.Contexts.Authentication;

public class AuthContext(IHttpContextAccessor accessor) : IAuthContext
{
    public bool IsAuthenticated => accessor.HttpContext!.IsAuthenticated() || accessor.HttpContext!.HasUserId();
    public Guid UserId => accessor.HttpContext!.GetUserId();
    public string Email => accessor.HttpContext!.GetUserEmail();
}