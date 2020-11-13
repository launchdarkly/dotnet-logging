using System;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// The underlying implementation object used by some <see cref="Logger"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Applications or libraries that generate log output do not need to interact directly with
    /// <see cref="IChannel"/>; implementations of it are created by whatever <see cref="ILogAdapter"/>
    /// is being used.
    /// </para>
    /// <para>
    /// The logger will send messages to this object, each with a <see cref="LogLevel"/>. If output is
    /// known to be completely disabled for the specified level, the <c>IChannel</c> method should
    /// return immediately and do no other processing. Otherwise, for simple messages it should call
    /// <c>ToString()</c> on the message parameter. It can always assume that <c>message</c> and
    /// <c>format</c> parameters are non-null.
    /// </para>
    /// <para>
    /// The reason that <c>format</c>/<c>param</c> values are passed straight through from <c>Logger</c>
    /// to <c>IChannel</c>, instead of having <c>Logger</c> do the string interpolation, is that an
    /// <see cref="IChannel"/> implementation that is delegating to another logging framework may not
    /// be able to know for sure whether a given log level is enabled (since filtering rules might be
    /// configured elsewhere in that framework); providing the parameters separately lets the
    /// implementation class decide whether or not to incur the overhead of string interpolation.
    /// </para>
    /// <para>
    /// The reason that there are four overloads for <c>Log()</c> is for efficiency, to avoid
    /// allocating a params array in the common case of a message with fewer than three parameters.
    /// </para>
    /// </remarks>
    public interface IChannel
    {
        /// <summary>
        /// Tests whether log output for a certain level is at least potentially visible.
        /// </summary>
        /// <remarks>
        /// This is the underlying implementation of <see cref="Logger.IsEnabled(LogLevel)"/>.
        /// The method should return true if the specified level is enabled in the sense that it will
        /// not be simply discarded by this <see cref="IChannel"/>. It should only return false if the
        /// <see cref="IChannel"/> will definitely discard that level.
        /// </remarks>
        /// <param name="level"></param>
        /// <returns>true if this level is potentially visible</returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// Logs a simple message with no parameters.
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="message">the message</param>
        void Log(LogLevel level, object message);

        /// <summary>
        /// Logs a message with a single parameter.
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="format">the format string</param>
        /// <param name="param">the parameter</param>
        void Log(LogLevel level, string format, Object param);

        /// <summary>
        /// Logs a message with two parameters.
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="format">the format string</param>
        /// <param name="param1">the first parameter</param>
        /// <param name="param2">the second parameter</param>
        void Log(LogLevel level, string format, object param1, object param2);

        /// <summary>
        /// Logs a message with any number of parameters.
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="format">the format string</param>
        /// <param name="allParams">the parameters</param>
        void Log(LogLevel level, String format, params object[] allParams);
    }
}
