using FluentMigrator;
using static Micro.Common.Infrastructure.Integration.Constants;

namespace Micro.Common.Infrastructure.Integration.Outbox;

public static class OutboxMigrationExtensions
{
    public static void CreateOutboxTable(this Migration migration)
    {
        migration.Create.Table(OutboxTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("type").AsString()
            .WithColumn("data").AsString()
            .WithColumn("created_at").AsDateTimeOffset()
            .WithColumn("processed_at").AsDateTimeOffset().Nullable();
    }

    public static void DropOutboxTable(this Migration migration)
    {
        migration.Delete.Table(OutboxTable).IfExists();
    }
}