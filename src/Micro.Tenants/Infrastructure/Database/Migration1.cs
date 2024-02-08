using FluentMigrator;

namespace Micro.Tenants.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table("organisations")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(100).Unique();

        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("organisation_id").AsGuid()
            .WithColumn("first_name").AsString(100)
            .WithColumn("last_name").AsString(100)
            .WithColumn("email").AsString(200).Unique()
            .WithColumn("password").AsString(100)
            .WithColumn("role").AsString(50);

        Create.ForeignKey("fk_users_organisations")
            .FromTable("users").ForeignColumn("organisation_id")
            .ToTable("organisations").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table("users").IfExists();
        Delete.Table("organisations").IfExists();
    }
}