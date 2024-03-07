using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Domain.Translations;
using static Micro.Translations.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Database;

public partial class Db : DbContext
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Language>().HaveConversion<LanguageCodeConverter>();
        configurationBuilder.Properties<ProjectId>().HaveConversion<ProjectIdConverter>();
        configurationBuilder.Properties<TermId>().HaveConversion<TermIdConverter>();
        configurationBuilder.Properties<TermName>().HaveConversion<TermNameConverter>();
        configurationBuilder.Properties<TranslationId>().HaveConversion<TranslationIdConverter>();
        configurationBuilder.Properties<TranslationText>().HaveConversion<TranslationTextConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Term>(entity =>
        {
            entity.ToTable(TermsTable, SchemaName);

            entity.HasIndex(e => new { e.ProjectId, e.Name }, "unique_terms_project_id_name").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);
            
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);
            
            entity.Property(e => e.ProjectId)
                .HasColumnName(ProjectIdColumn);
            
            
            // EF access the Translations collection property through its backing field
            // https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core
            entity.Metadata
                .FindNavigation(nameof(Term.Translations))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Translation>(entity =>
        {
            entity.ToTable(TranslationsTable, SchemaName);

            entity.HasIndex(e => new { e.TermId, LanguageId = e.Langauge }, "unique_translations_term_id_language_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);
            entity.Property(e => e.TermId)
                .HasColumnName(TermIdColumn);
            
            entity
                .Property(e => e.Langauge)
                .HasMaxLength(10)
                .HasColumnName(LanguageCodeColumn);
            
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnName(TextColumn);

            entity.HasOne(d => d.Term).WithMany(p => p.Translations)
                .HasForeignKey(d => d.TermId)
                .HasConstraintName("fk_translations_terms");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}