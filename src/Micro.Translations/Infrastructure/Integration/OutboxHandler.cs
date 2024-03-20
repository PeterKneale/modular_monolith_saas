using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Translations.Infrastructure.Integration;

public class OutboxHandler(Db db, OutboxMessagePublisher publisher, ILogger<OutboxHandler> log) : OutboxHandlerBase(db, publisher, log), IRequestHandler<ProcessOutboxCommand>
{
}