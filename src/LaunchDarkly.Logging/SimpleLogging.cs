using System;
using System.IO;
using System.Text;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// A basic logging implementation that sends preformatted output one line at a time
    /// to a <see cref="TextWriter"/>, or to any arbitrary output function.
    /// </summary>
    /// <remarks>
    /// This is the configurable adapter that is returned by <see cref="Logs.ToConsole"/>,
    /// <see cref="Logs.ToWriter(TextWriter)"/>, and <see cref="Logs.ToMethod(Action{string})"/>.
    /// You can specify additional options using the methods of this class, such as
    /// <see cref="DateFormat(string)"/>.
    /// </remarks>
    public class SimpleLogging : ILogAdapter
    {
        private readonly Action<string> _writeLine;
        private readonly string _dateFormat;

        /// <summary>
        /// The default format for log timestamps.
        /// </summary>
        public const string DefaultDateFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";

        internal SimpleLogging(Action<string> writeLine) :
            this(writeLine, DefaultDateFormat) { }

        internal SimpleLogging(Action<string> writeLine, string dateFormat)
        {
            _writeLine = writeLine;
            _dateFormat = dateFormat;
        }

        /// <summary>
        /// Specifies the format for date/timestamps, in the syntax used by <see cref="DateTime.ToString(string)"/>.
        /// </summary>
        /// <remarks>
        /// This method does not modify the current instance, but returns a new adapter based on this one.
        /// </remarks>
        /// <param name="dateFormat">the date/time format, or null to omit the date and time</param>
        /// <returns>an adapter with the specified configuration</returns>
        public SimpleLogging DateFormat(string dateFormat) =>
            new SimpleLogging(_writeLine, dateFormat);

        /// <summary>
        /// Called internally by the logging framework.
        /// </summary>
        /// <param name="name">the channel name</param>
        /// <returns>a new channel</returns>
        public IChannel NewChannel(string name) => new ChannelImpl(_writeLine, name, _dateFormat);

        private class ChannelImpl : IChannel
        {
            private readonly Action<string> _writeLine;
            private readonly string _name;
            private readonly string _dateFormat;

            internal ChannelImpl(Action<string> writeLine, string name, string dateFormat)
            {
                _writeLine = writeLine;
                _name = name;
                _dateFormat = dateFormat;
            }

            public bool IsEnabled(LogLevel level) => true;

            public void Log(LogLevel level, object message)
            {
                Print(level, message.ToString());
            }

            public void Log(LogLevel level, string format, object param)
            {
                Print(level, String.Format(format, param));
            }

            public void Log(LogLevel level, string format, object param1, object param2)
            {
                Print(level, String.Format(format, param1, param2));
            }

            public void Log(LogLevel level, string format, params object[] allParams)
            {
                Print(level, String.Format(format, allParams));
            }

            private void Print(LogLevel level, string message)
            {
                StringBuilder s = new StringBuilder();
                if (_dateFormat != null)
                {
                    s.Append(DateTime.Now.ToString(_dateFormat)).Append(" ");
                }
                s.Append("[").Append(_name).Append("] ").Append(level.Uppercase()).Append(": ").Append(message);
                _writeLine(s.ToString());
            }
        }
    }
}
