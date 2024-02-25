using FluentMigrator;
using Micro.Translations.Domain;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(TermsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("project_id").AsGuid()
            .WithColumn("name").AsString(100);

        Create.Table(TranslationsTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("term_id").AsGuid()
            .WithColumn("language_id").AsGuid()
            .WithColumn("text").AsString(100);
        
        Create.Table(LanguagesTable)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("project_id").AsGuid()
            .WithColumn("code").AsString(10);

        Create.ForeignKey($"fk_{TranslationsTable}_{TermsTable}")
            .FromTable(TranslationsTable).ForeignColumn("term_id")
            .ToTable(TermsTable).PrimaryColumn("id");
        
        Create.ForeignKey($"fk_{TranslationsTable}_{LanguagesTable}")
            .FromTable(TranslationsTable).ForeignColumn("language_id")
            .ToTable(LanguagesTable).PrimaryColumn("id");
        
        // Each language can only be added to a project once
        Create.UniqueConstraint($"unique_{LanguagesTable}_{ProjectIdColumn}_{CodeColumn}")
            .OnTable(LanguagesTable).Columns(ProjectIdColumn, CodeColumn);
        
        // Each term can only be added to a project once
        Create.UniqueConstraint($"unique_{TermsTable}_{ProjectIdColumn}_{NameColumn}")
            .OnTable(TermsTable).Columns(ProjectIdColumn, NameColumn);
        
        // Each translation can only be added to a term once per language
        Create.UniqueConstraint($"unique_{TranslationsTable}_{TermIdColumn}_{LanguageIdColumn}")
            .OnTable(TranslationsTable).Columns(TermIdColumn, LanguageIdColumn);
    }

    public override void Down()
    {
        Delete.Table(TranslationsTable).IfExists();
        Delete.Table(LanguagesTable).IfExists();
        Delete.Table(TermsTable).IfExists();
    }
}