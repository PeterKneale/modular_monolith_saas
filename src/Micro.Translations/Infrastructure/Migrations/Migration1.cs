using FluentMigrator;
using static Micro.Translations.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(TermsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(ProjectIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(100);

        Create.Table(TranslationsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(TermIdColumn).AsGuid()
            .WithColumn(LanguageCodeColumn).AsString(10)
            .WithColumn(TextColumn).AsString(100);

        Create.ForeignKey($"fk_{TranslationsTable}_{TermsTable}")
            .FromTable(TranslationsTable).ForeignColumn(TermIdColumn)
            .ToTable(TermsTable).PrimaryColumn(IdColumn);

        // Each term can only be added to a project once
        Create.UniqueConstraint($"unique_{TermsTable}_{ProjectIdColumn}_{NameColumn}")
            .OnTable(TermsTable).Columns(ProjectIdColumn, NameColumn);

        // Each translation can only be added to a term once per language
        Create.UniqueConstraint($"unique_{TranslationsTable}_{TermIdColumn}_{LanguageCodeColumn}")
            .OnTable(TranslationsTable).Columns(TermIdColumn, LanguageCodeColumn);
    }

    public override void Down()
    {
        Delete.Table(TranslationsTable).IfExists();
        Delete.Table(TermsTable).IfExists();
    }
}