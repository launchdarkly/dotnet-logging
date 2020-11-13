using System;
using System.IO;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Factory methods for the basic logging implementations in this package.
    /// </summary>
    /// <remarks>
    /// See <see cref="ILogAdapter"/> for more about how <c>LaunchDarkly.Logging</c> works with
    /// different implementations of logging. The methods and properties in <c>Logs</c> provide
    /// easy access to basic behaviors like logging to the console or to a file, or capturing log
    /// output for testing; if you need to direct the log output to another logging framework that
    /// your application is using, you will use an <see cref="ILogAdapter"/> implementation
    /// specific to that framework instead.
    /// </remarks>
    public static class Logs
    {
        /// <summary>
        /// A stub that generates no log output.
        /// </summary>
        public static ILogAdapter None => NullLogging.Instance;

        /// <summary>
        /// A default implementation that writes to the standard error stream at <c>Info</c> level.
        /// </summary>
        /// <remarks>
        /// This simply calls <c>ToConsole</c>, and then uses <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>
        /// to set the minimum level to <c>Info</c> (that is, it suppresses <c>Debug</c> logging).
        /// </remarks>
        public static ILogAdapter Default => ToConsole.Level(LogLevel.Info);

        /// <summary>
        /// A simple logging implementation that writes to the standard error stream.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is equivalent to <c>Logs.ToWriter(Console.Error)</c>.
        /// </para>
        /// <para>
        /// By default, all logging is enabled including <c>Debug</c>. To filter by level, use
        /// <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>.
        /// </para>
        /// </remarks>
        public static SimpleLogging ToConsole => ToWriter(Console.Error);

        /// <summary>
        /// A simple logging implementation that writes to any <c>TextWriter</c>.
        /// </summary>
        /// <remarks>
        /// This could be a built-in writer such as <c>Console.Out</c>, or a file.
        /// </remarks>
        /// <param name="stream">the destination for output</param>
        /// <returns></returns>
        public static SimpleLogging ToWriter(TextWriter stream) => new SimpleLogging(stream);

        /// <summary>
        /// A logging implementation that delegates to any number of destinations.
        /// </summary>
        /// <example>
        ///     // Send log output both to Console.Error and a file
        ///     var fileWriter = new StreamWriter("output.log");
        ///     var logAdapter = MultiLog.Adapter(
        ///         SimpleLog.Adapter, // writes to Console.Error
        /// </example>
        /// <param name="logAdapters"></param>
        /// <returns></returns>
        public static ILogAdapter ToMultiple(params ILogAdapter[] logAdapters) =>
            new MultiLogging(logAdapters);

        /// <summary>
        /// A logging implementation that captures log messages in memory.
        /// </summary>
        /// <returns>a <see cref="LogCapture"/> instance</returns>
        public static LogCapture Capture() => new LogCapture();
    }
}
