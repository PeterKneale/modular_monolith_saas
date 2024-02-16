using Micro.Common.Application;

namespace Micro.Common.Infrastructure.Context;

public interface IContextAccessor
{
    IUserExecutionContext? User { get; }
    
    IOrganisationExecutionContext? Organisation { get; }
    
    IProjectExecutionContext? Project { get; }
}