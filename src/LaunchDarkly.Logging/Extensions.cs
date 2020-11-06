using System;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Extension methods for convenience in implementing log adapters.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns an all-uppercase string describing the log level.
        /// </summary>
        /// <remarks>
        /// This is more efficient than <c>level.ToString().ToUpper()</c>.
        /// </remarks>
        /// <param name="level"></param>
        /// <returns>DEBUG, INFO, etc.</returns>
        public static string LevelUppercase(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.Info:
                    return "INFO";
                case LogLevel.Warn:
                    return "WARN";
                case LogLevel.Error:
                    return "ERROR";
                case LogLevel.None:
                    return "NONE";
                default:
                    return level.ToString().ToUpper();
            }
        }
    }
}
