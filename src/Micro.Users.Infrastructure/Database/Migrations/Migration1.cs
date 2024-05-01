using FluentMigrator;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
using static Micro.Users.Infrastructure.DbConstants;

namespace Micro.Users.Infrastructure.Database.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(UsersTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(FirstNameColumn).AsString(NameMaxLength)
            .WithColumn(LastNameColumn).AsString(NameMaxLength)
            .WithColumn(EmailColumn).AsString(EmailMaxLength).Unique()
            .WithColumn(PasswordColumn).AsString(NameMaxLength)
            .WithColumn(IsVerified).AsBoolean() // initially false
            .WithColumn(RegisteredAt).AsDateTimeOffset() 
            .WithColumn(VerifiedAt).AsDateTimeOffset().Nullable() // initially null then set on verification
            .WithColumn(VerifiedToken).AsString(50).Nullable() // initially set then cleared on verification
            .WithColumn(ForgotToken).AsString(50).Nullable()
            .WithColumn(ForgotPasswordTokenExpiry).AsDateTimeOffset().Nullable();

        Create.Table(UserApiKeysTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(UserIdColumn).AsGuid()
            .WithColumn(NameColumn).AsString(NameMaxLength)
            .WithColumn(KeyColumn).AsString(KeyMaxLength)
            .WithColumn(CreatedAtColumn).AsDateTimeOffset();

        Create.ForeignKey($"fk_{UserApiKeysTable}_{UsersTable}")
            .FromTable(UserApiKeysTable).ForeignColumn(UserIdColumn)
            .ToTable(UsersTable).PrimaryColumn(IdColumn);

        this.CreateInboxTable();
        this.CreateOutboxTable();
        this.CreateCommandTable();
    }

    public override void Down()
    {
        Delete.Table(UserApiKeysTable).IfExists();
        Delete.Table(UsersTable).IfExists();

        this.DropInboxTable();
        this.DropOutboxTable();
        this.DropCommandsTable();
    }
}