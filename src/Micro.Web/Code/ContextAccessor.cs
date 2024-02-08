using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Context;

namespace Micro.Web.Code;

public class ContextAccessor : IContextAccessor
{
    private readonly IHttpContextAccessor _accessor;

    public ContextAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public ICurrentContext? CurrentContext
    {
        get
        {
            var httpContext = _accessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            var identity = httpContext.User.Identity;
            if (identity is null)
            {
                return null;
            }

            var authenticated = identity.IsAuthenticated;
            if (!authenticated)
            {
                return null;
            }

            var organisationId = GetClaim(httpContext, "OrganisationId");
            if (organisationId == null)
            {
                return null;
            }

            var userId = GetClaim(httpContext, "UserId");
            if (userId == null)
            {
                return null;
            }

            return new CurrentContext(new OrganisationId(organisationId.Value), new UserId(userId.Value));
        }
    }

    private static Guid? GetClaim(HttpContext httpContext, string name)
    {
        var claim = httpContext.User.FindFirst(name);
        if (claim == null)
            return null;
        return Guid.Parse(claim.Value);
    }
}