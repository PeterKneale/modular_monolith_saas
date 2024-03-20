using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class OutboxHandler(Db db, OutboxMessagePublisher publisher, ILogger<OutboxHandler> log) : OutboxHandlerBase(db, publisher, log), IRequestHandler<ProcessOutboxCommand>;