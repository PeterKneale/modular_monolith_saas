using Micro.Common.Infrastructure.Database.Converters;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Users.Infrastructure.Database.Converters;
using static Micro.Users.Constants;

namespace Micro.Users.Infrastructure.Database;

public partial class Db : DbContext, IDbSetInbox, IDbSetOutbox, IDbSetQueue
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserApiKey> UserApiKeys { get; set; }

    public virtual DbSet<InboxMessage> Inbox { get; set; }

    public virtual DbSet<OutboxMessage> Outbox { get; set; }

    public virtual DbSet<QueueMessage> Queue { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
        configurationBuilder.Properties<UserApiKeyId>().HaveConversion<UserApiKeyIdConverter>();
        configurationBuilder.Properties<ApiKeyValue>().HaveConversion<ApiKeyValueConverter>();
        configurationBuilder.Properties<ApiKeyName>().HaveConversion<ApiKeyNameConverter>();
        configurationBuilder.Properties<EmailAddress>().HaveConversion<EmailAddressConverter>();
        configurationBuilder.Properties<Password>().HaveConversion<PasswordConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(UsersTable, SchemaName);

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

            entity.OwnsOne(x => x.Verification, x =>
            {
                x.Property(e => e.IsVerified).HasColumnName(IsVerified);
                x.Property(e => e.VerifiedAt).HasColumnName(VerifiedAt);
                x.Property(e => e.VerificationToken)
                    .HasMaxLength(50)
                    .HasColumnName(VerifiedToken);
            });

            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<UserApiKey>(entity =>
        {
            entity.ToTable(UserApiKeysTable, SchemaName);

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

        modelBuilder.AddInbox(SchemaName);
        modelBuilder.AddOutbox(SchemaName);
        modelBuilder.AddQueue(SchemaName);
        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}