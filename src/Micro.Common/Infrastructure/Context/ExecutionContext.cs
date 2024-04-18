using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ExecutionContext : IExecutionContext
{
    private readonly UserId? _userId;
    private readonly OrganisationId? _organisationId;
    private readonly ProjectId? _projectId;

    public ExecutionContext(UserId? userId = null, OrganisationId? organisationId = null, ProjectId? projectId = null)
    {
        _userId = userId;
        _organisationId = organisationId;
        _projectId = projectId;
    }

    public ExecutionContext(Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        if (userId != null)
        {
            _userId = new UserId(userId.Value);
        }

        if (organisationId != null)
        {
            _organisationId = new OrganisationId(organisationId.Value);
        }

        if (projectId != null)
        {
            _projectId = new ProjectId(projectId.Value);
        }
    }

    public UserId UserId => _userId ?? throw new ExecutionContextException("User ID is not set in the execution context");

    public OrganisationId OrganisationId => _organisationId ?? throw new ExecutionContextException("Organisation ID is not set in the execution context");

    public ProjectId ProjectId => _projectId ?? throw new ExecutionContextException("Project ID is not set in the execution context");
}