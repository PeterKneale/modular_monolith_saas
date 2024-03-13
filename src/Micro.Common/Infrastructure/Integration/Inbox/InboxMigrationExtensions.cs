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
            .WithColumn("created_at").AsDateTime()
            .WithColumn("processed_at").AsDateTime().Nullable();
    }

    public static void DropInboxTable(this Migration migration)
    {
        migration.Delete.Table("inbox").IfExists();
    }
}