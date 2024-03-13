using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessOutboxCommandHandler(Db db, OutboxMessagePublisher publisher) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await db.Outbox
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
        }

        return Unit.Value;
    }
}