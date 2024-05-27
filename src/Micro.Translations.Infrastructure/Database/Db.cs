using Micro.Common.Infrastructure.Database.Converters;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Translations.Domain;
using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Infrastructure.Database.Converters;
using static Micro.Translations.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Database;

public class Db : DbContext, IDbSetInbox, IDbSetOutbox, IDbSetQueue
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<Language> Languages { get; init; } = null!;

    public virtual DbSet<Term> Terms { get; init; } = null!;

    public virtual DbSet<Translation> Translations { get; init; } = null!;

    public virtual DbSet<User> Users { get; init; } = null!;

    public virtual DbSet<Project> Projects { get; init; } = null!;

    public virtual DbSet<InboxMessage> Inbox { get; init; } = null!;

    public virtual DbSet<OutboxMessage> Outbox { get; init; } = null!;

    public virtual DbSet<QueueMessage> Queue { get; init; } = null!;

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<LanguageId>().HaveConversion<LanguageIdConverter>();
        configurationBuilder.Properties<LanguageDetail>().HaveConversion<LanguageDetailConverter>();
        configurationBuilder.Properties<ProjectId>().HaveConversion<ProjectIdConverter>();
        configurationBuilder.Properties<TermId>().HaveConversion<TermIdConverter>();
        configurationBuilder.Properties<TermName>().HaveConversion<TermNameConverter>();
        configurationBuilder.Properties<TranslationId>().HaveConversion<TranslationIdConverter>();
        configurationBuilder.Properties<TranslationText>().HaveConversion<TranslationTextConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable(LanguagesTable, SchemaName);

            entity.Property(e => e.LanguageId)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.Property(e => e.ProjectId)
                .HasColumnName(ProjectIdColumn);

            entity.OwnsOne(x => x.Detail, x =>
            {
                x.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName(NameColumn);
                x.Property(e => e.Code)
                    .HasMaxLength(100)
                    .HasColumnName(CodeColumn);
            });
        });

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

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.Property(e => e.TermId)
                .HasColumnName(TermIdColumn);

            entity.Property(e => e.LanguageId)
                .HasColumnName(LanguageIdColumn);

            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnName(TextColumn);

            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(UsersTable, SchemaName);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable(ProjectsTable, SchemaName);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);
        });

        modelBuilder.AddInbox(SchemaName);
        modelBuilder.AddOutbox(SchemaName);
        modelBuilder.AddQueue(SchemaName);
    }
}