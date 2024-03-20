using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class InboxHandler(Db db, IPublisher publisher, ILogger<InboxHandlerBase> log) : InboxHandlerBase(db, publisher, log), IRequestHandler<ProcessInboxCommand>;