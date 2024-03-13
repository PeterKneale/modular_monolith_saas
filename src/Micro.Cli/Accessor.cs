using Micro.Common.Application;

namespace Micro.Cli
{
    public class Accessor : IExecutionContextAccessor
    {
        public IExecutionContext ExecutionContext { get; }
    }
}