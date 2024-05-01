namespace Micro.Tenants.Infrastructure;

public static class DbConstants
{
    public const string SchemaName = "tenants";

    // Table names
    public const string OrganisationsTable = "organisations";
    public const string MembershipsTable = "memberships";
    public const string UsersTable = "users";
    public const string ProjectsTable = "projects";

    // Column Names
    public const string IdColumn = "id";
    public const string NameColumn = "name";
    public const string CreatedAt = "created_at";
    public const string UpdatedAt = "updated_at";
    public const string OrganisationIdColumn = "organisation_id";
    public const string UserIdColumn = "user_id";
    public const string RoleColumn = "role";

    // Other Constants
    public const int NameMaxLength = 100;
    public const int RoleMaxLength = 100;
}