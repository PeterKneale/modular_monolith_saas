using Micro.Common.Infrastructure.Database.Converters;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Users.Domain.ApiKeys;
using Micro.Users.Domain.Users;
using Micro.Users.Infrastructure.Database.Converters;
using static Micro.Users.Infrastructure.DbConstants;

namespace Micro.Users.Infrastructure.Database;

public class Db : DbContext, IDbSetInbox, IDbSetOutbox, IDbSetQueue
{
    public Db()
    {
    }

    public Db(DbContextOptions<Db> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; init; } = null!;

    public virtual DbSet<UserApiKey> UserApiKeys { get; init; } = null!;

    public virtual DbSet<InboxMessage> Inbox { get; init; } = null!;

    public virtual DbSet<OutboxMessage> Outbox { get; init; } = null!;

    public virtual DbSet<QueueMessage> Queue { get; init; } = null!;

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
        configurationBuilder.Properties<UserApiKeyId>().HaveConversion<UserApiKeyIdConverter>();
        configurationBuilder.Properties<ApiKeyValue>().HaveConversion<ApiKeyValueConverter>();
        configurationBuilder.Properties<ApiKeyName>().HaveConversion<ApiKeyNameConverter>();
        configurationBuilder.Properties<Password>().HaveConversion<PasswordConverter>();
        configurationBuilder.Properties<PasswordHash>().HaveConversion<PasswordHashConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(UsersTable, SchemaName);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.OwnsOne(x => x.Name, name =>
            {
                name.Property(property => property.First)
                    .HasMaxLength(NameMaxLength)
                    .HasColumnName(FirstNameColumn);

                name.Property(property => property.Last)
                    .HasMaxLength(NameMaxLength)
                    .HasColumnName(LastNameColumn);
            });

            entity.OwnsOne(x => x.EmailAddress, email =>
            {
                email.Property(property => property.Canonical)
                    .HasMaxLength(EmailMaxLength)
                    .HasColumnName(EmailCanonicalColumn);

                email.Property(property => property.Display)
                    .HasMaxLength(EmailMaxLength)
                    .HasColumnName(EmailDisplayColumn);
            });

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .HasColumnName(PasswordColumn);

            entity.Property(e => e.RegisteredAt).HasColumnName(RegisteredAt);

            entity.Property(e => e.IsVerified).HasColumnName(IsVerified);
            entity.Property(e => e.VerifiedAt).HasColumnName(VerifiedAt);
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(50)
                .HasColumnName(VerifiedToken);

            entity.Property(e => e.ForgotPasswordToken)
                .HasMaxLength(50)
                .HasColumnName(ForgotToken);
            entity.Property(e => e.ForgotPasswordTokenExpiry)
                .HasColumnName(ForgotPasswordTokenExpiry);

            entity.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<UserApiKey>(entity =>
        {
            entity.ToTable(UserApiKeysTable, SchemaName);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName(IdColumn);

            entity.OwnsOne(x => x.ApiKey, x =>
            {
                x.Property(e => e.Key)
                    .HasMaxLength(KeyMaxLength)
                    .HasColumnName(KeyColumn);

                x.Property(e => e.Name)
                    .HasMaxLength(NameMaxLength)
                    .HasColumnName(NameColumn);

                x.Property(e => e.CreatedAt)
                    .HasColumnName(CreatedAtColumn);
            });

            entity.Property(e => e.UserId)
                .HasColumnName(UserIdColumn);

            entity.HasOne(d => d.User).WithMany(p => p.UserApiKeys)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.AddInbox(SchemaName);
        modelBuilder.AddOutbox(SchemaName);
        modelBuilder.AddQueue(SchemaName);
    }
}