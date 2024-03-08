using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Domain.ApiKeys;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Projects;
using Micro.Tenants.Domain.Users;
using Micro.Tenants.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Database;

public partial class Db : DbContext
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserApiKey> UserApiKeys { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<OrganisationId>().HaveConversion<OrganisationIdConverter>();
        configurationBuilder.Properties<OrganisationName>().HaveConversion<OrganisationNameConverter>();
        configurationBuilder.Properties<MembershipId>().HaveConversion<MembershipIdConverter>();
        configurationBuilder.Properties<MembershipRole>().HaveConversion<MembershipRoleConverter>();
        configurationBuilder.Properties<ProjectId>().HaveConversion<ProjectIdConverter>();
        configurationBuilder.Properties<ProjectName>().HaveConversion<ProjectNameConverter>();
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();        
        configurationBuilder.Properties<UserApiKeyId>().HaveConversion<UserApiKeyIdConverter>();
        configurationBuilder.Properties<ApiKeyValue>().HaveConversion<ApiKeyValueConverter>();
        configurationBuilder.Properties<ApiKeyName>().HaveConversion<ApiKeyNameConverter>();
        configurationBuilder.Properties<EmailAddress>().HaveConversion<EmailAddressConverter>();
        configurationBuilder.Properties<Password>().HaveConversion<PasswordConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.ToTable("organisations", "tenants");

            entity.HasIndex(e => e.Name, "IX_organisations_name").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.ToTable("memberships", "tenants");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .HasColumnName("role");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.OrganisationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_memberships_organisations");

            entity.HasOne(d => d.User).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_memberships_users");
            
            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("projects", "tenants");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.OrganisationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_projects_organisations");
            
            entity.Ignore(x => x.DomainEvents);
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users", "tenants");

            //entity.HasIndex(e => e.Credentials.Email, "IX_users_email").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.OwnsOne(x => x.Name, x =>
            {
                x.Property(e => e.First)
                    .HasMaxLength(100)
                    .HasColumnName("first_name");
                x.Property(e => e.Last)
                    .HasMaxLength(100)
                    .HasColumnName("last_name");
            });
            entity.OwnsOne(x => x.Credentials, x =>
            {
                x.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("email");
                x.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");
            });
            
            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<UserApiKey>(entity =>
        {
            entity.ToTable("user_api_keys", "tenants");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.OwnsOne(x => x.ApiKey, x =>
            {
                x.Property(e => e.Key)
                    .HasMaxLength(100)
                    .HasColumnName("key");
                x.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
                x.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");
            });
            
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserApiKeys)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_api_keys_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}