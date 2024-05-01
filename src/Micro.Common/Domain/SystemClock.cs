namespace Micro.Common.Domain;

public static class SystemClock
{
    private static DateTimeOffset? _customDate;

    public static DateTimeOffset UtcNow => _customDate ?? DateTime.UtcNow;

    public static void Set(DateTimeOffset customDate)
    {
        _customDate = customDate;
    }

    public static void Reset() => _customDate = null;
}