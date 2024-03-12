using System.Text.RegularExpressions;

namespace Micro.Common.Domain;

public static class NameSanitizer
{
    // Sanitize the value to contain only alphanumeric characters, underscores, and dashes
    public static string SanitizedValue(string value) => Regex.Replace(value, "[^a-zA-Z0-9-_]", "-");
}