using FluentMigrator;

namespace Micro.Translations.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table("terms")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("organisation_id").AsGuid()
            .WithColumn("app_id").AsGuid()
            .WithColumn("name").AsString(100);

        Create.Table("translations")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("term_id").AsGuid()
            .WithColumn("language_code").AsString(10)
            .WithColumn("text").AsString(100);

        Create.ForeignKey("fk_translations_terms")
            .FromTable("translations").ForeignColumn("term_id")
            .ToTable("terms").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table("translations").IfExists();
        Delete.Table("terms").IfExists();
    }
}