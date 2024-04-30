using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Micro.Translations.Infrastructure.temp;

public partial class Db : DbContext
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<Inbox> Inboxes { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Outbox> Outboxes { get; set; }

    public virtual DbSet<Queue> Queues { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Username=admin;Password=password;Database=db;Host=localhost;Port=5432;Search Path=translate;Include Error Detail=true;Log Parameters=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inbox>(entity =>
        {
            entity.ToTable("inbox", "translate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("languages", "translate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(100)
                .HasColumnName("language_code");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
        });

        modelBuilder.Entity<Outbox>(entity =>
        {
            entity.ToTable("outbox", "translate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Queue>(entity =>
        {
            entity.ToTable("queue", "translate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.ToTable("terms", "translate");

            entity.HasIndex(e => new { e.ProjectId, e.Name }, "unique_terms_project_id_name").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
        });

        modelBuilder.Entity<Translation>(entity =>
        {
            entity.ToTable("translations", "translate");

            entity.HasIndex(e => new { e.TermId, e.LanguageId }, "unique_translations_term_id_language_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.TermId).HasColumnName("term_id");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnName("text");

            entity.HasOne(d => d.Language).WithMany(p => p.Translations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_translations_languages");

            entity.HasOne(d => d.Term).WithMany(p => p.Translations)
                .HasForeignKey(d => d.TermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_translations_terms");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users", "translate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo", "translate");

            entity.HasIndex(e => e.Version, "UC_Version").IsUnique();

            entity.Property(e => e.AppliedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
