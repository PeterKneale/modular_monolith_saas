using Micro.Common.Infrastructure.Integration.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration;

public interface IInboxDbSet
{
    public DbSet<InboxMessage> Inbox { get; }
}