using Micro.Common.Infrastructure.Integration.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration;

public interface IDbSetOutbox
{
    public DbSet<OutboxMessage> Outbox { get; }
}