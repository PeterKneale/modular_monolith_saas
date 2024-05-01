using FluentMigrator;
using static Micro.Common.Infrastructure.Integration.Constants;

namespace Micro.Common.Infrastructure.Integration.Inbox;

public static class InboxMigrationExtensions
{
    public static void CreateInboxTable(this Migration migration)
    {
        migration.Create.Table(InboxTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("type").AsString()
            .WithColumn("data").AsString()
            .WithColumn("created_at").AsDateTimeOffset()
            .WithColumn("processed_at").AsDateTimeOffset().Nullable();
    }

    public static void DropInboxTable(this Migration migration)
    {
        migration.Delete.Table(InboxTable).IfExists();
    }
}