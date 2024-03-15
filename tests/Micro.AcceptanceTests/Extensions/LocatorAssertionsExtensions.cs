namespace Micro.AcceptanceTests.Extensions;

public static class LocatorAssertionsExtensions
{
    private const string CssClassIsValid = "is-valid";

    private const string CssClassIsInvalid = "is-invalid";
    
    public static Task ToBe(this ILocatorAssertions assertions, bool valid) =>
        valid ? assertions.ToBeValid() : assertions.ToBeInvalid();

    private static Task ToBeValid(this ILocatorAssertions assertions) =>
        assertions.ToHaveClassAsync(new Regex(CssClassIsValid));

    private static Task ToBeInvalid(this ILocatorAssertions assertions) =>
        assertions.ToHaveClassAsync(new Regex(CssClassIsInvalid));

    public static bool ToBoolean(this string validity) =>
        validity.ToLowerInvariant() switch
        {
            "valid" => true,
            "true" => true,
            "invalid" => false,
            "false" => false,
            _ => throw new NotSupportedException($"Validity {validity} is not supported")
        };
}