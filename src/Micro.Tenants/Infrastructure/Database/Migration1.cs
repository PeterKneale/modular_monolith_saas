﻿using FluentMigrator;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Database;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(OrganisationsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString(NameMaxLength).Unique();

        Create.Table(MembershipsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(OrganisationIdColumn).AsGuid()
            .WithColumn(UserIdColumn).AsGuid()
            .WithColumn(RoleColumn).AsString(RoleMaxLength);
        
        Create.Table(UsersTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(FirstNameColumn).AsString(NameMaxLength)
            .WithColumn(LastNameColumn).AsString(NameMaxLength)
            .WithColumn(EmailColumn).AsString(EmailMaxLength).Unique()
            .WithColumn(PasswordColumn).AsString(NameMaxLength);

        Create.ForeignKey($"fk_{MembershipsTable}_{OrganisationsTable}")
            .FromTable(MembershipsTable).ForeignColumn(OrganisationIdColumn)
            .ToTable(OrganisationsTable).PrimaryColumn(IdColumn);
        
        Create.ForeignKey($"fk_{MembershipsTable}_{UsersTable}")
            .FromTable(MembershipsTable).ForeignColumn(UserIdColumn)
            .ToTable(UsersTable).PrimaryColumn(IdColumn);
    }

    public override void Down()
    {
        Delete.Table(MembershipsTable).IfExists();
        Delete.Table(UsersTable).IfExists();
        Delete.Table(OrganisationsTable).IfExists();
    }
}