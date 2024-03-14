using FluentMigrator;

namespace Micro.Common.Infrastructure.Integration.Inbox;

public static class InboxMigrationExtensions
{
    public static void CreateInboxTable(this Migration migration)
    {
        migration.Create.Table("inbox")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("type").AsString()
            .WithColumn("data").AsString()
            .WithColumn("created_at").AsDateTimeOffset()
            .WithColumn("processed_at").AsDateTimeOffset().Nullable();
    }

    public static void DropInboxTable(this Migration migration)
    {
        migration.Delete.Table("inbox").IfExists();
    }
}