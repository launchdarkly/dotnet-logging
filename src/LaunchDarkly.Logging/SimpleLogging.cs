using System;
using System.IO;
using System.Text;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// A basic logging implementation that sends output to any <see cref="TextWriter"/>.
    /// </summary>
    /// <remarks>
    /// This is the configurable adapter that is returned by <see cref="Logs.ToConsole"/>
    /// or <see cref="Logs.ToWriter(TextWriter)"/>. You can specify additional options
    /// using the methods of this class, such as <see cref="DateFormat(string)"/>.
    /// </remarks>
    public class SimpleLogging : ILogAdapter
    {
        private readonly TextWriter _stream;
        private readonly string _dateFormat;

        /// <summary>
        /// The default format for log timestamps.
        /// </summary>
        public const string DefaultDateFormat = "yyyy-MM-dd HH:mm:ss.SSS zzz";

        internal SimpleLogging(TextWriter stream) :
            this(stream, DefaultDateFormat) { }

        internal SimpleLogging(TextWriter stream, string dateFormat)
        {
            _stream = stream;
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
            new SimpleLogging(_stream, dateFormat);

        /// <summary>
        /// Called internally by the logging framework.
        /// </summary>
        /// <param name="name">the channel name</param>
        /// <returns>a new channel</returns>
        public IChannel NewChannel(string name) => new ChannelImpl(_stream, name, _dateFormat);

        private class ChannelImpl : IChannel
        {
            private readonly TextWriter _stream;
            private readonly string _name;
            private readonly string _dateFormat;

            internal ChannelImpl(TextWriter stream, string name, string dateFormat)
            {
                _stream = stream;
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
                _stream.WriteLine(s.ToString());
            }
        }
    }
}
