using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Context;

namespace Micro.Web.Code;

public class ContextAccessor(IHttpContextAccessor accessor) : IContextAccessor
{
    public IUserContext? User => UserId.HasValue ? new UserContext(new UserId(UserId.Value)) : null;
    public IOrganisationContext? Organisation => OrganisationId.HasValue ? new OrganisationContext(new OrganisationId(OrganisationId.Value)) : null;

    private Guid? UserId => GetClaim(accessor.HttpContext, "UserId");
    private Guid? OrganisationId => GetItem(accessor.HttpContext, "OrganisationId");

    private static Guid? GetItem(HttpContext? context, string name)
    {
        var item = context?.Items[name];
        if (item == null)
            return null;
        return Guid.Parse(item.ToString()!);
    }

    private static Guid? GetClaim(HttpContext? context, string name)
    {
        var identity = context?.User.Identity;
        if (identity is null)
        {
            return null;
        }

        var authenticated = identity.IsAuthenticated;
        if (!authenticated)
        {
            return null;
        }

        var claim = context.User.FindFirst(name);
        if (claim == null)
            return null;

        return Guid.Parse(claim.Value);
    }
}