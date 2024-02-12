using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class ProjectContext(ProjectId projectId) : IProjectContext
{
    public ProjectId ProjectId => projectId;
}