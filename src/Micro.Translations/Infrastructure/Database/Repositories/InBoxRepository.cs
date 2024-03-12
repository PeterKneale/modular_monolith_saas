﻿using Micro.Common.Infrastructure.Inbox;
using Micro.Common.Infrastructure.Integration;

namespace Micro.Translations.Infrastructure.Database.Repositories;

internal class InboxRepository(Db db) : IInboxRepository
{
    public async Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token)
    {
        var type = integrationEvent.GetType().Name;
        var data = JsonConvert.SerializeObject(integrationEvent, Formatting.Indented);
        var record = new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Data = data
        };
        await db.Inbox.AddAsync(record, token);
    }
}