using System;
using System.IO;

#if NETCOREAPP
using Microsoft.Extensions.Logging;
#endif

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
        /// By default, all logging is enabled including <c>Debug</c> level. To filter by level, use
        /// <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>. You can also use
        /// <see cref="SimpleLogging"/> methods for additional configuration.
        /// </para>
        /// </remarks>
        public static SimpleLogging ToConsole => ToWriter(Console.Error);

        /// <summary>
        /// A simple logging implementation that writes to any <c>TextWriter</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This could be a built-in writer such as <c>Console.Out</c>, or a file.
        /// </para>
        /// <para>
        /// By default, all logging is enabled including <c>Debug</c> level. To filter by level, use
        /// <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>. You can also use
        /// <see cref="SimpleLogging"/> methods for additional configuration.
        /// </para>
        /// </remarks>
        /// <param name="stream">the destination for output</param>
        /// <returns>a configurable logging adapter</returns>
        public static SimpleLogging ToWriter(TextWriter stream) => new SimpleLogging(stream);

#if NETCOREAPP
        /// <summary>
        /// A logging implementation that delegates to the .NET Core logging framework.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is only available when your target framework is .NET Core. It causes
        /// the <c>LaunchDarkly.Logging</c> APIs to delegate to the <c>Microsoft.Extensions.Logging</c>
        /// framework. The <c>ILoggingFactory</c> is the main configuration object for
        /// <c>Microsoft.Extensions.Logging</c>; application code can construct it programmatically,
        /// or can obtain it by dependency injection. For more information, see
        /// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1">Logging
        /// in .NET Core and ASP.NET Core</see>.
        /// </para>
        /// <para>
        /// The .NET Core logging framework has its own mechanisms for filtering log output
        /// by level or other criteria. If you add a level filter with
        /// <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>, it will filter
        /// out messages below that level before they reach the .NET Core logger.
        /// </para>
        /// </remarks>
        /// <param name="loggerFactory">the factory object for .NET Core logging</param>
        /// <returns>a logging adapter</returns>
        public static ILogAdapter CoreLogging(ILoggerFactory loggerFactory) =>
            new NetCoreLogging(loggerFactory);
#endif

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
        /// <returns>an <see cref="ILogAdapter"/></returns>
        public static ILogAdapter ToMultiple(params ILogAdapter[] logAdapters) =>
            new MultiLogging(logAdapters);

        /// <summary>
        /// A logging implementation that captures log messages in memory.
        /// </summary>
        /// <returns>a <see cref="LogCapture"/> instance</returns>
        public static LogCapture Capture() => new LogCapture();
    }
}
