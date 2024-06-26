﻿using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Web.Contexts.PageContext;
using Micro.Users.Web.Contexts.Authentication;
using static Micro.Common.Infrastructure.Context.ExecutionContext;

namespace Micro.Web.Code.Contexts.ExecutionContext;

public class HttpExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public IExecutionContext ExecutionContext
    {
        get
        {
            var authContext = new AuthContext(httpContextAccessor);
            var pageContext = new PageContextAccessor(httpContextAccessor);
            
            Guid? userId = authContext.IsAuthenticated
                ? authContext.UserId
                : null;
            
            Guid? organisationId = pageContext.HasOrganisation 
                ? pageContext.Organisation.Id
                : null;
            
            Guid? projectId = pageContext.HasProject 
                ? pageContext.Project.Id
                : null;

            return Create(userId, organisationId, projectId);
        }
    }
}