using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IProjectContext
{
    ProjectId ProjectId { get; }
}