using Micro.Common.Application;

namespace Micro.Common.Infrastructure.Context;

public interface IContextAccessor
{
    IUserContext? User { get; }
    IOrganisationContext? Organisation { get; }
}