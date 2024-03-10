namespace Micro.Tenants;

public static class Constants
{
    public const string SchemaName = "tenants";
    public const string OrganisationsTable = "organisations";
    public const string MembershipsTable = "memberships";
    public const string UsersTable = "users";
    public const string ProjectsTable = "projects";
    public const string UserApiKeysTable = "user_api_keys";

    // Common Column Names
    public const string IdColumn = "id";
    public const string NameColumn = "name";
    public const string FirstNameColumn = "first_name";
    public const string LastNameColumn = "last_name";
    public const string EmailColumn = "email";
    public const string PasswordColumn = "password";
    public const string OrganisationIdColumn = "organisation_id";
    public const string UserIdColumn = "user_id";
    public const string RoleColumn = "role";
    public const string CreatedAtColumn = "created_at";
    public const string KeyColumn = "key";

    // Other Constants
    public const int NameMaxLength = 100;
    public const int KeyMaxLength = 100;
    public const int EmailMaxLength = 200;
    public const int RoleMaxLength = 100;
}