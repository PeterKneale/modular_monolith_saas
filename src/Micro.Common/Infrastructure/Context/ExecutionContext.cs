using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ExecutionContext : IExecutionContext
{
    private ExecutionContext(UserId? userId, OrganisationId? organisationId, ProjectId? projectId)
    {
        UserId = userId!;
        OrganisationId = organisationId!;
        ProjectId = projectId != null ? new ProjectId(projectId.Value) : null!;
    }

    public UserId UserId { get; }
    public OrganisationId OrganisationId { get; }
    public ProjectId ProjectId { get; }

    public static ExecutionContext Create(UserId userId, OrganisationId organisationId, ProjectId projectId) =>
        new(userId, organisationId, projectId);

    public static ExecutionContext Create(Guid? userId = null, Guid? organisationId = null, Guid? projectId = null)
    {
        var x = userId != null ? new UserId(userId.Value) : null;
        var y = organisationId != null ? new OrganisationId(organisationId.Value) : null;
        var z = projectId != null ? new ProjectId(projectId.Value) : null;
        return new ExecutionContext(x, y, z);
    }

    public static ExecutionContext Empty() =>
        new(null, null, null);
}