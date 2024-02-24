namespace Micro.Web.Code.Contexts.Authentication;

public class AuthContext(IHttpContextAccessor accessor) : IAuthContext
{
    public bool IsAuthenticated => accessor.HttpContext!.IsAuthenticated();
    public Guid UserId => accessor.HttpContext!.GetUserId();
    public string Email => accessor.HttpContext!.GetUserEmail();
}