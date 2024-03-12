using FluentMigrator;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Database.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(OrganisationsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString(NameMaxLength).Unique();

        Create.Table(MembershipsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(UserIdColumn).AsGuid()
            .WithColumn(RoleColumn).AsString(RoleMaxLength);

        Create.Table(UsersTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(FirstNameColumn).AsString(NameMaxLength)
            .WithColumn(LastNameColumn).AsString(NameMaxLength)
            .WithColumn(EmailColumn).AsString(EmailMaxLength).Unique()
            .WithColumn(PasswordColumn).AsString(NameMaxLength);

        Create.Table(ProjectsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(NameMaxLength);

        Create.Table(UserApiKeysTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(UserIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(NameMaxLength)
            .WithColumn(KeyColumn).AsString(KeyMaxLength)
            .WithColumn(CreatedAtColumn).AsDateTimeOffset();

        Create.ForeignKey($"fk_{MembershipsTable}_{OrganisationsTable}")
            .FromTable(MembershipsTable).ForeignColumn(OrganisationIdColumn)
            .ToTable(OrganisationsTable).PrimaryColumn(IdColumn);

        Create.ForeignKey($"fk_{MembershipsTable}_{UsersTable}")
            .FromTable(MembershipsTable).ForeignColumn(UserIdColumn)
            .ToTable(UsersTable).PrimaryColumn(IdColumn);

        Create.ForeignKey($"fk_{ProjectsTable}_{OrganisationsTable}")
            .FromTable(ProjectsTable).ForeignColumn(OrganisationIdColumn)
            .ToTable(OrganisationsTable).PrimaryColumn(IdColumn);

        Create.ForeignKey($"fk_{UserApiKeysTable}_{UsersTable}")
            .FromTable(UserApiKeysTable).ForeignColumn(UserIdColumn)
            .ToTable(UsersTable).PrimaryColumn(IdColumn);

        this.CreateInboxTable();
        this.CreateOutboxTable();
    }

    public override void Down()
    {
        Delete.Table(UserApiKeysTable).IfExists();
        Delete.Table(ProjectsTable).IfExists();
        Delete.Table(MembershipsTable).IfExists();
        Delete.Table(UsersTable).IfExists();
        Delete.Table(OrganisationsTable).IfExists();

        this.DropInboxTable();
        this.DropOutboxTable();
    }
}