using Microsoft.Extensions.Logging;

namespace Typewriter
{
    public class Log
    {
        public static ILogger Logger;
        internal static void Warn(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        internal static void Error(string message, params object[] args)
        {
            Logger.LogError(message, args);
        }

        internal static void Debug(string message, params object[] args)
        {
            Logger.LogDebug(message, args);
        }

        internal static void Information(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }
    }
}
