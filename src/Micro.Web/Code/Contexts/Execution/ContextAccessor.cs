using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Web.Code.Contexts.Authentication;

namespace Micro.Web.Code.Contexts.Execution;

public class ContextAccessor(IHttpContextAccessor accessor) : IContextAccessor
{
    public IUserExecutionContext? User
    {
        get
        {
            var context = new AuthContext(accessor);
            return context.IsAuthenticated 
                ? new UserExecutionContext(new UserId(context.UserId)) 
                : null;
        }
    }

    public IOrganisationExecutionContext? Organisation
    {
        get
        {
            var context = new PageContextAccessor(accessor);
            return context.HasOrganisation 
                ? new OrganisationExecutionContext(new OrganisationId(context.Organisation.Id)) 
                : null;
        }
    }

    public IProjectExecutionContext? Project
    {
        get
        {
            var context = new PageContextAccessor(accessor);
            return context.HasProject 
                ? new ProjectExecutionContext(new ProjectId(context.Project.Id)) 
                : null;
        }
    }
}