namespace Micro.Web.Code;

public class Alert
{
    public string Message { get; init; }
    public AlertLevel Level { get; init; }

    public Alert()
    {
        // for deserialisation    
    }

    private Alert(string message, AlertLevel level)
    {
        Message = message;
        Level = level;
    }

    public enum AlertLevel
    {
        Info,
        Success,
        Warning,
        Danger
    }

    public string CssClass => $"alert alert-{Enum.GetName(Level)!.ToLower()}";

    public static Alert Info(string message) => new(message, AlertLevel.Info);
    public static Alert Success(string message) => new(message, AlertLevel.Success);
    public static Alert Warning(string message) => new(message, AlertLevel.Warning);
    public static Alert Danger(string message) => new(message, AlertLevel.Danger);
}