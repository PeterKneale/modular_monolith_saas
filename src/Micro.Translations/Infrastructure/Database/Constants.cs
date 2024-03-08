namespace Micro.Translations.Infrastructure.Database;

public static class Constants
{
    // Schema
    public const string SchemaName = "translate";

    // tables
    public const string TermsTable = "terms";
    public const string TranslationsTable = "translations";

    // columns
    public const string IdColumn = "id";
    public const string TermIdColumn = "term_id";
    public const string ProjectIdColumn = "project_id";
    public const string NameColumn = "name";
    public const string LanguageCodeColumn = "language_code";
    public const string TextColumn = "text";
}