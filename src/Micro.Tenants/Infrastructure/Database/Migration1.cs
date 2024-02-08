using FluentMigrator;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(OrganisationsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(100).Unique();

        Create.Table(UsersTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("organisation_id").AsGuid()
            .WithColumn("first_name").AsString(100)
            .WithColumn("last_name").AsString(100)
            .WithColumn("email").AsString(200).Unique()
            .WithColumn("password").AsString(100)
            .WithColumn("role").AsString(50);

        Create.ForeignKey($"fk_{UsersTable}_{OrganisationsTable}")
            .FromTable(UsersTable).ForeignColumn("organisation_id")
            .ToTable(OrganisationsTable).PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table(UsersTable).IfExists();
        Delete.Table(OrganisationsTable).IfExists();
    }
}