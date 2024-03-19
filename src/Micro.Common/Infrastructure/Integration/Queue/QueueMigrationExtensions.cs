using FluentMigrator;

namespace Micro.Common.Infrastructure.Integration.Queue;

public static class QueueMigrationExtensions
{
    public static void CreateCommandTable(this Migration migration)
    {
        migration.Create.Table("queue")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("type").AsString()
            .WithColumn("data").AsString()
            .WithColumn("created_at").AsDateTimeOffset()
            .WithColumn("processed_at").AsDateTimeOffset().Nullable();
    }

    public static void DropCommandsTable(this Migration migration)
    {
        migration.Delete.Table("queue").IfExists();
    }
}