namespace Micro.Users;

public static class Constants
{
    public const string SchemaName = "users";
    public const string UsersTable = "users";
    public const string UserApiKeysTable = "user_api_keys";

    // Common Column Names
    public const string CreatedAtColumn = "created_at";
    public const string EmailColumn = "email";
    public const string FirstNameColumn = "first_name";
    public const string IdColumn = "id";
    public const string IsVerified = "is_verified";
    public const string KeyColumn = "key";
    public const string LastNameColumn = "last_name";
    public const string NameColumn = "name";
    public const string PasswordColumn = "password";
    public const string UserIdColumn = "user_id";
    public const string VerifiedAt = "verified_at";
    public const string VerifiedToken = "verification_token";
    public const string ForgotToken = "forgot_token";
    public const string ForgotPasswordTokenExpiry = "forgot_token_expiry";

    // Other Constants
    public const int NameMaxLength = 100;
    public const int KeyMaxLength = 100;
    public const int EmailMaxLength = 200;
}