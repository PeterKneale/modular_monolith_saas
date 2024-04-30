using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ExecutionContext : IExecutionContext
{
    private readonly UserId? _userId;
    private readonly OrganisationId? _organisationId;
    private readonly ProjectId? _projectId;

    private ExecutionContext(UserId? userId = null, OrganisationId? organisationId = null, ProjectId? projectId = null)
    {
        _userId = userId;
        _organisationId = organisationId;
        _projectId = projectId;
    }

    private ExecutionContext(Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        if (userId != null)
        {
            _userId = UserId.Create(userId.Value);
        }

        if (organisationId != null)
        {
            _organisationId = OrganisationId.Create(organisationId.Value);
        }

        if (projectId != null)
        {
            _projectId = ProjectId.Create(projectId.Value);
        }
    }

    public static ExecutionContext Create(UserId? userId = null, OrganisationId? organisationId = null, ProjectId? projectId = null) =>
        new(userId, organisationId, projectId);

    public static ExecutionContext Create(Guid? userId = null, Guid? organisationId = null, Guid? projectId = null) =>
        new(userId, organisationId, projectId);

    public UserId UserId => _userId ?? throw new ExecutionContextException("User ID is not set in the execution context");

    public OrganisationId OrganisationId => _organisationId ?? throw new ExecutionContextException("Organisation ID is not set in the execution context");

    public ProjectId ProjectId => _projectId ?? throw new ExecutionContextException("Project ID is not set in the execution context");
}