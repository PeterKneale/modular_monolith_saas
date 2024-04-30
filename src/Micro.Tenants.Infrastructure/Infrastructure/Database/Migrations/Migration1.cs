using static Micro.Tenants.Infrastructure.DbConstants;

namespace Micro.Tenants.Infrastructure.Infrastructure.Database.Migrations;

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
            .WithColumn(UserIdColumn).AsGuid() // Loosely coupled
            .WithColumn(RoleColumn).AsString(RoleMaxLength);

        Create.Table(UsersTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString(100);

        Create.Table(ProjectsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(NameMaxLength);

        Create.ForeignKey($"fk_{MembershipsTable}_{OrganisationsTable}")
            .FromTable(MembershipsTable).ForeignColumn(OrganisationIdColumn)
            .ToTable(OrganisationsTable).PrimaryColumn(IdColumn);

        Create.ForeignKey($"fk_{ProjectsTable}_{OrganisationsTable}")
            .FromTable(ProjectsTable).ForeignColumn(OrganisationIdColumn)
            .ToTable(OrganisationsTable).PrimaryColumn(IdColumn);

        this.CreateInboxTable();
        this.CreateOutboxTable();
        this.CreateCommandTable();
    }

    public override void Down()
    {
        Delete.Table(ProjectsTable).IfExists();
        Delete.Table(MembershipsTable).IfExists();
        Delete.Table(UsersTable).IfExists();
        Delete.Table(OrganisationsTable).IfExists();

        this.DropInboxTable();
        this.DropOutboxTable();
        this.DropCommandsTable();
    }
}