using FluentMigrator;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using static Micro.Translations.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Database.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(LanguagesTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(ProjectIdColumn).AsGuid()
            .WithColumn(LanguageCodeColumn).AsString(100);
        
        Create.Table(TermsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(ProjectIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(100);

        Create.Table(TranslationsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(TermIdColumn).AsGuid()
            .WithColumn(LanguageIdColumn).AsGuid()
            .WithColumn(TextColumn).AsString(100);

        Create.Table(UsersTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString(100);
        
        Create.Table(ProjectsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString(100);
        
        Create.ForeignKey($"fk_{TranslationsTable}_{TermsTable}")
            .FromTable(TranslationsTable).ForeignColumn(TermIdColumn)
            .ToTable(TermsTable).PrimaryColumn(IdColumn);
        
        Create.ForeignKey($"fk_{TranslationsTable}_{LanguagesTable}")
            .FromTable(TranslationsTable).ForeignColumn(LanguageIdColumn)
            .ToTable(LanguagesTable).PrimaryColumn(IdColumn);

        // Each term can only be added to a project once
        Create.UniqueConstraint($"unique_{TermsTable}_{ProjectIdColumn}_{NameColumn}")
            .OnTable(TermsTable).Columns(ProjectIdColumn, NameColumn);

        // Each translation can only be added to a term once per language
        Create.UniqueConstraint($"unique_{TranslationsTable}_{TermIdColumn}_{LanguageIdColumn}")
            .OnTable(TranslationsTable).Columns(TermIdColumn, LanguageIdColumn);

        this.CreateInboxTable();
        this.CreateOutboxTable();
        this.CreateCommandTable();
    }

    public override void Down()
    {
        Delete.Table(TranslationsTable).IfExists();
        Delete.Table(TermsTable).IfExists();
        Delete.Table(LanguagesTable).IfExists();
        Delete.Table(ProjectsTable).IfExists();
        Delete.Table(UsersTable).IfExists();

        this.DropInboxTable();
        this.DropOutboxTable();
        this.DropCommandsTable();
    }
}