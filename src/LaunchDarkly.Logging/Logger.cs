using System;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// A basic logger facade that delegates to an underlying output implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Code that generates log output will send it through this class. What happens to
    /// the output depends on the <see cref="ILogAdapter"/> that was used to create the
    /// <see cref="Logger"/>.
    /// </para>
    /// <para>
    /// The logger has output methods for each of the levels defined in <see cref="LogLevel"/>.
    /// Each can take either a simple string, or a format string with variable parameters in
    /// the syntax used by <see cref="String.Format(string, object)"/>. For efficiency (to
    /// avoid unnecessarily creating varargs arrays), each method has four overloads: one
    /// simple string, format with one parameter, format with two parameters, and format
    /// with an arbitrary number of parameters.
    /// </para>
    /// </remarks>
    public sealed class Logger
    {
        private readonly string _name;
        private readonly ILogAdapter _adapter;
        private readonly IChannel _channel;

        internal Logger(string name, ILogAdapter adapter, IChannel channel)
        {
            _name = name;
            _adapter = adapter;
            _channel = channel;
        }

        /// <summary>
        /// Creates a named logger instance using the specified adapter.
        /// </summary>
        /// <remarks>
        /// This method (or the equivalent shortcut <c>adapter.Logger(name)</c>) is called by
        /// library code to acquire a <c>Logger</c> instance that it will write output to.
        /// </remarks>
        /// <param name="adapter">the <see cref="ILogAdapter"/> that defines the actual logging
        /// implementation</param>
        /// <param name="name">the name for this logger</param>
        /// <returns>a new logger instance</returns>
        public static Logger WithAdapter(ILogAdapter adapter, string name)
        {
            return new Logger(name, adapter, adapter.NewChannel(name));
        }

        /// <summary>
        /// Creates a logger instance derived from this instance.
        /// </summary>
        /// <param name="nameSuffix">will be appended to the current logger's name, separated by a
        /// period, to create the new logger's name</param>
        /// <returns>a new logger instance</returns>
        public Logger SubLogger(string nameSuffix)
        {
            if (nameSuffix is null || nameSuffix == "")
            {
                return this;
            }
            string subName = _name + "." + nameSuffix;
            return new Logger(subName, _adapter, _adapter.NewChannel(subName));
        }

        /// <summary>
        /// Tests whether log output for a certain level is at least potentially visible.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Generally, any desired level filtering should be set up in the initial logging
        /// configuration, and code that generates log messages should simply call methods like
        /// <see cref="Logger.Info(object)"/> without having to know whether that particular level
        /// is enabled or is being filtered out. However, if some kind of log message is particularly
        /// expensive to compute, you may call <c>IsEnabled</c>; a false value means you can skip
        /// trying to log any message at that level.
        /// </para>
        /// <para>
        /// Another approach is to generate any computationally expensive output lazily, such as by
        /// using the methods in <see cref="LogValues"/>.
        /// </para>
        /// </remarks>
        /// <param name="level">a log level</param>
        /// <returns>true if this level is potentially visible</returns>
        public bool IsEnabled(LogLevel level) => _channel.IsEnabled(level);

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">the message; if null, nothing is logged</param>
        public void Debug(object message)
        {
            if (!(message is null))
            {
                _channel.Log(LogLevel.Debug, message);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Debug"/> level with one parameter.
        /// </summary>
        /// <param name="format">the format string; if null, nothing is logged</param>
        /// <param name="param">the parameter</param>
        public void Debug(string format, object param)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Debug, format, param);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Debug"/> level with two parameters.
        /// </summary>
        /// <param name="format">the format string; if null, nothing is logged</param>
        /// <param name="param1">the first parameter</param>
        /// <param name="param2">the second parameter</param>
        public void Debug(string format, object param1, object param2)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Debug, format, param1, param2);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Debug"/> level with any number of parameters.
        /// </summary>
        /// <param name="format">the format string; if null, nothing is logged</param>
        /// <param name="allParams">the parameters</param>
        public void Debug(string format, params object[] allParams)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Debug, format, allParams);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">the message</param>
        public void Info(object message)
        {
            if (!(message is null))
            {
                _channel.Log(LogLevel.Info, message);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Info"/> level with one parameter.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param">the parameter</param>
        public void Info(string format, object param)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Info, format, param);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Info"/> level with two parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param1">the first parameter</param>
        /// <param name="param2">the second parameter</param>
        public void Info(string format, object param1, object param2)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Info, format, param1, param2);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Info"/> level with any number of parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="allParams">the parameters</param>
        public void Info(string format, params object[] allParams)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Info, format, allParams);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">the message</param>
        public void Warn(object message)
        {
            if (!(message is null))
            {
                _channel.Log(LogLevel.Warn, message);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Warn"/> level with one parameter.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param">the parameter</param>
        public void Warn(string format, object param)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Warn, format, param);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Warn"/> level with two parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param1">the first parameter</param>
        /// <param name="param2">the second parameter</param>
        public void Warn(string format, object param1, object param2)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Warn, format, param1, param2);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Warn"/> level with any number of parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="allParams">the parameters</param>
        public void Warn(string format, params object[] allParams)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Warn, format, allParams);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">the message</param>
        public void Error(object message)
        {
            if (!(message is null))
            {
                _channel.Log(LogLevel.Error, message);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Error"/> level with one parameter.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param">the parameter</param>
        public void error(string format, object param)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Error, format, param);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Error"/> level with two parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="param1">the first parameter</param>
        /// <param name="param2">the second parameter</param>
        public void Error(string format, object param1, object param2)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Error, format, param1, param2);
            }
        }

        /// <summary>
        /// Writes a message at <see cref="LogLevel.Error"/> level with any number of parameters.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="allParams">the parameters</param>
        public void Error(string format, params object[] allParams)
        {
            if (!(format is null))
            {
                _channel.Log(LogLevel.Error, format, allParams);
            }
        }
    }
}
