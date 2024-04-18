using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Web.Code.Contexts.Authentication;

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
                ? new UserId(authContext.UserId) 
                : null;
            
            var organisationId = pageContext.HasOrganisation 
                ? new OrganisationId(pageContext.Organisation.Id) 
                : null;
            
            var projectId = pageContext.HasProject 
                ? new ProjectId(pageContext.Project.Id) 
                : null;

            return new Micro.Common.Infrastructure.Context.ExecutionContext(userId, organisationId, projectId);
        }
    }
}