using FluentMigrator;

namespace Micro.Common.Infrastructure.Inbox;

public static class InboxMigrationExtensions
{
    public static void CreateInboxTable(this Migration migration)
    {
        migration.Create.Table("inbox")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("type").AsString()
            .WithColumn("data").AsString();
    }

    public static void DropInboxTable(this Migration migration)
    {
        migration.Delete.Table("inbox").IfExists();
    }
}