﻿namespace Micro.Translations.Infrastructure.Database;

public static class Constants
{
    public const string SchemaName = "translate";
    public const string TermsTable = "terms";
    public const string TranslationsTable = "translations";

    // Common Column Names
    public const string IdColumn = "id";
    public const string TermIdColumn = "term_id";
    public const string TranslationIdColumn = "translation_id";
    public const string ProjectIdColumn = "project_id";
    public const string NameColumn = "name";
    public const string LanguageCodeColumn = "language_code";
    public const string TextColumn = "text";

    // Other Constants
}