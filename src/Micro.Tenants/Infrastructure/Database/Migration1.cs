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

        Create.Table(MembershipsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("organisation_id").AsGuid()
            .WithColumn("user_id").AsGuid()
            .WithColumn("role").AsString(100);
        
        Create.Table(UsersTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("first_name").AsString(100)
            .WithColumn("last_name").AsString(100)
            .WithColumn("email").AsString(200).Unique()
            .WithColumn("password").AsString(100);

        Create.ForeignKey($"fk_{MembershipsTable}_{OrganisationsTable}")
            .FromTable(MembershipsTable).ForeignColumn("organisation_id")
            .ToTable(OrganisationsTable).PrimaryColumn("id");
        
        Create.ForeignKey($"fk_{MembershipsTable}_{UsersTable}")
            .FromTable(MembershipsTable).ForeignColumn("user_id")
            .ToTable(UsersTable).PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table(MembershipsTable).IfExists();
        Delete.Table(UsersTable).IfExists();
        Delete.Table(OrganisationsTable).IfExists();
    }
}