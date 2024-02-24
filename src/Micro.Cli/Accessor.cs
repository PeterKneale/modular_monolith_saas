using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;

namespace Micro.Cli
{
    public class Accessor : IContextAccessor
    {
        public IUserExecutionContext? User { get; set; }
        public IOrganisationExecutionContext? Organisation { get; set; }
        public IProjectExecutionContext? Project { get; set; }
    }
}