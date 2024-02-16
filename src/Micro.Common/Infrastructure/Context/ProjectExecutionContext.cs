using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ProjectExecutionContext(ProjectId projectId) : IProjectExecutionContext
{
    public ProjectId ProjectId => projectId;
}