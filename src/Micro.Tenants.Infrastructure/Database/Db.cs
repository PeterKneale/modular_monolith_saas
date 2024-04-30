using Micro.Tenants.Infrastructure.Database.Converters;
using static Micro.Tenants.Infrastructure.DbConstants;

namespace Micro.Tenants.Infrastructure.Database;

public class Db : DbContext, IDbSetInbox, IDbSetOutbox, IDbSetQueue
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

    public virtual DbSet<InboxMessage> Inbox { get; set; }

    public virtual DbSet<OutboxMessage> Outbox { get; set; }

    public virtual DbSet<QueueMessage> Queue { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<MembershipId>().HaveConversion<MembershipIdConverter>();
        configurationBuilder.Properties<MembershipRole>().HaveConversion<MembershipRoleConverter>();
        configurationBuilder.Properties<OrganisationId>().HaveConversion<OrganisationIdConverter>();
        configurationBuilder.Properties<OrganisationName>().HaveConversion<OrganisationNameConverter>();
        configurationBuilder.Properties<ProjectId>().HaveConversion<ProjectIdConverter>();
        configurationBuilder.Properties<ProjectName>().HaveConversion<ProjectNameConverter>();
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.ToTable(OrganisationsTable, SchemaName);

            entity.HasIndex(e => e.Name, "IX_organisations_name").IsUnique();

            entity.Property(e => e.OrganisationId)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);

            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.ToTable(MembershipsTable, SchemaName);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.Property(e => e.OrganisationId)
                .HasColumnName(OrganisationIdColumn);

            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .HasColumnName(RoleColumn);

            entity.Property(e => e.UserId)
                .HasColumnName(UserIdColumn);

            entity
                .HasOne(d => d.Organisation)
                .WithMany(p => p.Memberships)
                .HasForeignKey(d => d.OrganisationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_memberships_organisations");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Memberships)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_memberships_users");

            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity
                .ToTable(ProjectsTable, SchemaName);

            entity
                .Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity
                .Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);

            entity
                .Property(e => e.OrganisationId)
                .HasColumnName(OrganisationIdColumn);

            entity
                .HasOne(d => d.Organisation)
                .WithMany(p => p.Projects)
                .HasForeignKey(d => d.OrganisationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_projects_organisations");

            entity.Ignore(x => x.DomainEvents);
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity
                .ToTable(UsersTable, SchemaName);

            entity
                .Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity
                .Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName(NameColumn);
        });


        modelBuilder.AddInbox(SchemaName);
        modelBuilder.AddOutbox(SchemaName);
        modelBuilder.AddQueue(SchemaName);
    }
}