namespace Micro.Common.Web.Contexts.AuthContext;

public interface IAuthContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Email { get; }
}