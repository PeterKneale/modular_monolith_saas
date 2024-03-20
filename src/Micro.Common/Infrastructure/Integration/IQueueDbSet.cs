using Micro.Common.Infrastructure.Integration.Queue;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration;

public interface IQueueDbSet
{
    public DbSet<QueueMessage> Commands { get; }
}