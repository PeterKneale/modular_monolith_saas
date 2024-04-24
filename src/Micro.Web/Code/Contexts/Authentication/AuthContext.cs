namespace Micro.Web.Code.Contexts.Authentication;

public interface IAuthContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Email { get; }
}

public class AuthContext(IHttpContextAccessor accessor) : IAuthContext
{
    public bool IsAuthenticated => accessor.HttpContext!.IsAuthenticated() || accessor.HttpContext!.HasUserId();
    public Guid UserId => accessor.HttpContext!.GetUserId();
    public string Email => accessor.HttpContext!.GetUserEmail();
}