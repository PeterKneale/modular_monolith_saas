using Micro.Common.Application;

namespace Micro.Common.Infrastructure.Context;

public interface IExecutionContextAccessor
{
    public IExecutionContext ExecutionContext { get; }
}

public class SettableExecutionContextAccessor : IExecutionContextAccessor
{
    public IExecutionContext ExecutionContext { get; set; }
}