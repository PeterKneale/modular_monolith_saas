using System.Diagnostics;
using Polly;

namespace Micro.Web.AcceptanceTests.Extensions;

public static class EventuallyHelper
{
    public static async Task ShouldPass(Func<Task> action, string? message = "unknown") =>
        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                10,
                _ => TimeSpan.FromMilliseconds(100),
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .ExecuteAsync(action);

    public static void ShouldPass(Action action, string? message = "unknown") =>
        Policy
            .Handle<Exception>()
            .WaitAndRetry(
                10,
                _ => TimeSpan.FromMilliseconds(100),
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .Execute(action);

    public static async Task ShouldPassAsync(Func<Task> action, string? message = "unknown") =>
        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                10,
                _ => TimeSpan.FromMilliseconds(100),
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .ExecuteAsync(action);

    private static void Log(int retryNo, Exception result, TimeSpan timespan, string? message)
    {
        var test = TestContext.CurrentContext.Test.Name;
        var log = $"Waiting for condition '{message}' in test {test}. RetryNo: {retryNo}. Message: {result.Message}. Delaying: {timespan.TotalSeconds} seconds";
        TestContext.WriteLine(log);
        Trace.WriteLine(log);
        Debug.WriteLine(log);
        Console.WriteLine(log);
        
    }
}