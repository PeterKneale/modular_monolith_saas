using Micro.Common.Application;

namespace Micro.Common.Infrastructure.Context;

public interface IContextAccessor
{
    ICurrentContext? CurrentContext { get; }
}