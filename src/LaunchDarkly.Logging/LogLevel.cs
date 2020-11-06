
namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Enumeration of the logging levels defined by the LaunchDarkly.Logging abstraction.
    /// </summary>
    /// <remarks>
    /// This is the same basic level concept that exists in most logging frameworks. Levels
    /// are ranked in ascending order from <see cref="Debug"/> to <see cref="Error"/>. Whatever
    /// minimum level is enabled for the logger, any messages at a lower level will be
    /// suppressed: for instance, if the minimum level is <see cref="Warn"/>, then there will be
    /// no output for <see cref="Debug"/> or <see cref="Info"/>.
    /// </remarks>
    public enum LogLevel
    {
        /// <summary>
        /// This level is for very detailed and verbose messages that are rarely useful except
        /// in diagnosing an unusual problem.
        /// </summary>
        Debug,

        /// <summary>
        /// This level is for informational messages that are logged during normal operation.
        /// </summary>
        Info,

        /// <summary>
        /// This level is for messages about unexpected conditions that may be worth noting,
        /// but that do not necessarily prevent things from working.
        /// </summary>
        Warn,

        /// <summary>
        /// This level is for errors that should not happen during normal operation
        /// and should be investigated.
        /// </summary>
        Error,

        /// <summary>
        /// This level is not used for output; setting the minimum enabled level to None
        /// disables all output.
        /// </summary>
        None
    }

    /// <summary>
    /// Extension methods for convenience in implementing log adapters.
    /// </summary>
    public static class LogLevelExtensions
    {
        /// <summary>
        /// Returns an all-uppercase string describing the log level.
        /// </summary>
        /// <remarks>
        /// This is more efficient than <c>level.ToString().ToUpper()</c>.
        /// </remarks>
        /// <param name="level"></param>
        /// <returns>DEBUG, INFO, etc.</returns>
        public static string Uppercase(this LogLevel level)
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
