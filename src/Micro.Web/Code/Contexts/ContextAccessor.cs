using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Web.Code.PageContext;

namespace Micro.Web.Code.Contexts;

public class ContextAccessor(IHttpContextAccessor accessor) : IContextAccessor
{
    public IUserContext? User => GetUser(accessor);

    public IOrganisationContext? Organisation => GetOrganisation(accessor);

    public IProjectContext? Project => GetProject(accessor);

    private static IUserContext? GetUser(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        var userId = context.GetUserId();
        return userId.HasValue ? new UserContext(new UserId(userId.Value)) : null;
    }

    private static IOrganisationContext? GetOrganisation(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        if (!context.HasOrganisationContext())
        {
            return null;
        }

        var organisationContext = context.GetOrganisationContext();
        return new OrganisationContext(new OrganisationId(organisationContext.Id));
    }

    private static IProjectContext? GetProject(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        if (!context.HasOrganisationContext())
        {
            return null;
        }

        var projectContext = context.GetProjectContext();
        return new ProjectContext(new ProjectId(projectContext.Id));
    }
}