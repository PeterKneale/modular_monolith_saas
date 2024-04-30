﻿using Micro.Common.Infrastructure.Database.Converters;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Translations.Domain.LanguageAggregate;
using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Domain.UserAggregate;
using Micro.Translations.Infrastructure.Infrastructure.Database.Converters;
using static Micro.Translations.Infrastructure.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Infrastructure.Database;

public class Db : DbContext, IDbSetInbox, IDbSetOutbox, IDbSetQueue
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<InboxMessage> Inbox { get; set; }

    public virtual DbSet<OutboxMessage> Outbox { get; set; }

    public virtual DbSet<QueueMessage> Queue { get; set; }

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
            
            entity.OwnsOne(x=>x.Detail)
                .Property(x=>x.Code)
                .HasMaxLength(10)
                .HasColumnName(LanguageCodeColumn);
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

        modelBuilder.AddInbox(SchemaName);
        modelBuilder.AddOutbox(SchemaName);
        modelBuilder.AddQueue(SchemaName);
    }
}