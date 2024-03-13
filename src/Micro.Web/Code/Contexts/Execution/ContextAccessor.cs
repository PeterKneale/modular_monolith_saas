using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Web.Code.Contexts.Authentication;

namespace Micro.Web.Code.Contexts.Execution;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
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

            return Common.Infrastructure.Context.ExecutionContext.Create(userId, organisationId, projectId);
        }
    }
}