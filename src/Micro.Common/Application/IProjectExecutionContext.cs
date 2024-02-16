using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IProjectExecutionContext
{
    ProjectId ProjectId { get; }
}