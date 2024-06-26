﻿using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.IntegrationTests.Fixtures;

public static class IntegrationHelper
{
    public static async Task PurgeInbox()
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        foreach (var message in await db.Inbox.ToListAsync()) db.Inbox.Remove(message);
        await db.SaveChangesAsync();
    }

    public static async Task PurgeOutbox()
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        foreach (var message in await db.Outbox.ToListAsync()) db.Outbox.Remove(message);
        await db.SaveChangesAsync();
    }

    public static async Task PushMessageIntoInbox(IIntegrationEvent integrationEvent)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        await db.Inbox.AddAsync(InboxMessage.CreateFrom(integrationEvent));
        await db.SaveChangesAsync();
    }

    public static async Task PushMessageIntoOutbox(IIntegrationEvent integrationEvent)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        await db.Outbox.AddAsync(OutboxMessage.CreateFrom(integrationEvent));
        await db.SaveChangesAsync();
    }

    public static async Task<int> CountPendingInboxMessages()
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        return await db.Inbox.CountAsync(x => x.ProcessedAt == null);
    }

    public static async Task<int> CountPendingOutboxMessages()
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        return await db.Outbox.CountAsync(x => x.ProcessedAt == null);
    }
}