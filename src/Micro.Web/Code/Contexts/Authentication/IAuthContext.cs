namespace Micro.Web.Code.Contexts.Authentication;

public interface IAuthContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Email { get; }
}