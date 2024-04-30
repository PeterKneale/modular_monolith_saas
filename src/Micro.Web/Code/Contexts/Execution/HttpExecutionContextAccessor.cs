using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Web.Code.Contexts.Authentication;
using static Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Web.Code.Contexts.Execution;

public class HttpExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public IExecutionContext ExecutionContext
    {
        get
        {
            var authContext = new AuthContext(httpContextAccessor);
            var pageContext = new PageContextAccessor(httpContextAccessor);
            
            var userId = authContext.IsAuthenticated 
                ? UserId.Create(authContext.UserId) 
                : null;
            
            var organisationId = pageContext.HasOrganisation 
                ? OrganisationId.Create(pageContext.Organisation.Id) 
                : null;
            
            var projectId = pageContext.HasProject 
                ? ProjectId.Create(pageContext.Project.Id) 
                : null;

            return Create(userId, organisationId, projectId);
        }
    }
}