using Micro.Common.Application;

namespace Micro.Cli
{
    public class Accessor : IContextAccessor
    {
        public IUserExecutionContext? User { get; set; }
        public IOrganisationExecutionContext? Organisation { get; set; }
        public IProjectExecutionContext? Project { get; set; }
    }
}