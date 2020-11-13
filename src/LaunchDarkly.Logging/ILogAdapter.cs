
namespace LaunchDarkly.Logging
{
    /// <summary>
    /// An abstraction of some mechanism for producing log output.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Any LaunchDarkly library that can generate log output through <c>LaunchDarkly.Logging</c>
    /// has a configuration option of type <see cref="ILogAdapter"/>, which defines the
    /// implementation details of what to do with the log output. Built-in basic implementations
    /// are available through the <see cref="Logs"/> class, and adapters that delegate to other
    /// logging frameworks can be provided by other LaunchDarkly packages or by the application.
    /// </para>
    /// <para>
    /// The basic model is that whatever component will be writing to the logs will define at
    /// least one name for an output channel. The adapter's <see cref="NewChannel(string)"/> method
    /// takes a name and returns a low-level <see cref="IChannel"/> implementation that accepts log
    /// messages for any <see cref="LogLevel"/>; this is wrapped in the standard <see cref="Logger"/>
    /// class, which is what the rest of the LaunchDarkly library code interacts with.
    /// </para>
    /// <para>
    /// Applications should not need to interact directly with <see cref="ILogAdapter"/>, beyond
    /// the initial configuration step of choosing which one to use.
    /// </para>
    /// </remarks>
    public interface ILogAdapter
    {
        /// <summary>
        /// The logger calls this method to obtain a named output channel.
        /// </summary>
        /// <remarks>
        /// The name will be included in all log output for this channel. Channels are meant to be
        /// retained and reused by the components they belong to, so the ILogAdapter does not need
        /// to cache them.
        /// </remarks>
        /// <param name="name">an identifying name</param>
        /// <returns>an implementation of <see cref="IChannel"/></returns>
        IChannel NewChannel(string name);
    }

    /// <summary>
    /// Extension methods that can be applied to any <see cref="ILogAdapter"/>.
    /// </summary>
    public static class ILogAdapterExtensions
    {
        /// <summary>
        /// Disables log output below the specified level.
        /// </summary>
        /// <remarks>
        /// This is a decorator that can be applied to any <see cref="ILogAdapter"/>, either one of
        /// the standard ones available in <see cref="Logs"/> or a custom implementation. Any log
        /// messages for a lower level will be immediately discarded; all others will be forwarded to
        /// the underlying logging implementation (which may also have other filtering rules of its
        /// own).
        /// </remarks>
        /// <example>
        ///     // This one will write all log messages to Console.Error, including Debug messages
        ///     var unfilteredLogging = Logs.ToConsole;
        ///
        ///     // This one will write only Warn and Error messages
        ///     var filteredLogging = Logs.ToConsole.Level(Warn);
        /// </example>
        /// <param name="adapter"></param>
        /// <param name="minimumLevel"></param>
        /// <returns>an <see cref="ILogAdapter"/> that wraps the original one but filters out log
        /// messages of lower levels</returns>
        public static ILogAdapter Level(this ILogAdapter adapter, LogLevel minimumLevel)
        {
            return new LevelFilter(adapter, minimumLevel);
        }

        /// <summary>
        /// Convenience method for creating logger instances.
        /// </summary>
        /// <remarks>
        /// This is a shortcut for calling <see cref="LaunchDarkly.Logging.Logger.WithAdapter(ILogAdapter, string)"/>.
        /// Application code will not normally use this method; it is used by library code
        /// to set up individual named loggers that a library will log to.
        /// </remarks>
        /// <param name="adapter">the <see cref="ILogAdapter"/> that provides the
        /// underlying implementation of logging</param>
        /// <param name="name">the name for this logger</param>
        /// <returns>a logger instance</returns>
        public static Logger Logger(this ILogAdapter adapter, string name) =>
            Logging.Logger.WithAdapter(adapter, name);
    }
}
