using Quartz.Logging;
using LogLevel = Quartz.Logging.LogLevel;

namespace Micro.Common.Infrastructure.Jobs;

public class QuartzConsoleLogger : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            if (level >= LogLevel.Info && func != null) Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
            return true;
        };
    }

    public IDisposable OpenNestedContext(string message) => throw new NotImplementedException();

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false) => throw new NotImplementedException();
}