using FluentMigrator;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(TermsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("organisation_id").AsGuid()
            .WithColumn("app_id").AsGuid()
            .WithColumn("name").AsString(100);

        Create.Table(TranslationsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("term_id").AsGuid()
            .WithColumn("language_code").AsString(10)
            .WithColumn("text").AsString(100);

        Create.ForeignKey($"fk_{TranslationsTable}_{TermsTable}")
            .FromTable(TranslationsTable).ForeignColumn("term_id")
            .ToTable(TermsTable).PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table(TranslationsTable).IfExists();
        Delete.Table(TermsTable).IfExists();
    }
}