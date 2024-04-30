using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ExecutionContext : IExecutionContext
{
    private readonly UserId _userId;
    private readonly OrganisationId _organisationId;
    private readonly ProjectId _projectId;

    private ExecutionContext(UserId userId, OrganisationId organisationId, ProjectId projectId)
    {
        _userId = userId;
        _organisationId = organisationId;
        _projectId = projectId;
    }

    public static ExecutionContext Create(Guid? userId = null, Guid? organisationId = null, Guid? projectId = null) =>
        new(UserId.Create(userId), OrganisationId.Create(organisationId), ProjectId.Create(projectId));

    public UserId UserId =>
        _userId != UserId.Empty ? _userId : throw new ExecutionContextException("User ID is not set in the execution context");

    public OrganisationId OrganisationId =>
        _organisationId != OrganisationId.Empty ? _organisationId : throw new ExecutionContextException("Organisation ID is not set in the execution context");

    public ProjectId ProjectId =>
        _projectId != ProjectId.Empty ? _projectId : throw new ExecutionContextException("Project ID is not set in the execution context");
}