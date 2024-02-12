using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;

namespace Micro.Web.Code;

public class ContextAccessor(IHttpContextAccessor accessor) : IContextAccessor
{
    public IUserContext? User => UserId.HasValue ? new UserContext(new UserId(UserId.Value)) : null;
    
    public IOrganisationContext? Organisation => OrganisationId.HasValue ? new OrganisationContext(new OrganisationId(OrganisationId.Value)) : null;
    
    public IProjectContext? Project => ProjectId.HasValue ? new ProjectContext(new ProjectId(ProjectId.Value)) : null;

    private Guid? UserId => accessor.HttpContext!.GetUserId();
    
    private Guid? OrganisationId => accessor.HttpContext!.GetOrganisationId();
    
    private Guid? ProjectId => accessor.HttpContext!.GetProjectId();
}