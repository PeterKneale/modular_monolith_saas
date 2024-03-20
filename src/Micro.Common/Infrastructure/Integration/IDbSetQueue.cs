using Micro.Common.Infrastructure.Integration.Queue;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration;

public interface IDbSetQueue
{
    public DbSet<QueueMessage> Queue { get; }
}