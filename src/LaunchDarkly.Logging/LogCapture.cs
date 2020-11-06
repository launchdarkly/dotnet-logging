using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// A mechanism for capturing logger output in memory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Calling <see cref="Logs.Capture()"/> provides a <c>LogCapture</c> object that accumulates
    /// all log output from any code that is configured to use it as the log adapter. This is mainly
    /// intended for testing.
    /// </para>
    /// <para>
    /// All messages that come to this object are captured regardless of the log level. If you want
    /// to filter out messages below a certain level, you can apply <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>
    /// and pass the resulting filtered adapter to whatever component will be doing the logging, in
    /// place of the original <see cref="LogCapture"/> object.
    /// </para>
    /// </remarks>
    /// <example>
    ///     var logSink = Logs.Capture();
    ///     // ... execute some code that produces log output to this adapter
    ///     var capturedMessages = sink.GetMessages();
    /// </example>
    public class LogCapture : ILogAdapter
    {
        private readonly List<Message> _messages = new List<Message>();

        /// <summary>
        /// Called internally by the logging framework.
        /// </summary>
        /// <param name="name">the channel name</param>
        /// <returns>a new channel</returns>
        public IChannel NewChannel(string name) => new ChannelImpl(_messages, name);

        /// <summary>
        /// Returns all captured messages.
        /// </summary>
        /// <returns>a copy of the messages</returns>
        public List<Message> GetMessages()
        {
            lock (_messages)
            {
                return new List<Message>(_messages);
            }
        }

        /// <summary>
        /// Returns all captured messages converted to strings, in the format "[LoggerName] LEVEL: text".
        /// </summary>
        /// <returns>a copy of the messages as strings</returns>
        public List<string> GetMessageStrings()
        {
            lock (_messages)
            {
                return new List<string>(_messages.Select(m => m.ToString()));
            }
        }

        /// <summary>
        /// Tests whether any captured message for the given level exactly matches the given text.
        /// </summary>
        /// <param name="level">a log level</param>
        /// <param name="text">the desired message text</param>
        /// <returns></returns>
        public bool HasMessageWithText(LogLevel level, string text)
        {
            lock (_messages)
            {
                return _messages.Any(m => m.Level == level && m.Text == text);
            }
        }

        /// <summary>
        /// Tests whether any captured message for the given level matches the given regular expression.
        /// </summary>
        /// <param name="level">a log level</param>
        /// <param name="pattern">a regular expression</param>
        /// <returns></returns>
        public bool HasMessageWithRegex(LogLevel level, string pattern)
        {
            lock (_messages)
            {
                return _messages.Any(m => m.Level == level && Regex.IsMatch(m.Text, pattern));
            }
        }

        /// <summary>
        /// Returns all of the captured log output as a string.
        /// </summary>
        /// <returns>a string containing all captured log lines</returns>
        public override string ToString()
        {
            return string.Join("\n", GetMessageStrings());
        }

        /// <summary>
        /// Information about a captured log message.
        /// </summary>
        public struct Message
        {
            /// <summary>
            /// The name of the logger that produced the message.
            /// </summary>
            public string LoggerName { get; }

            /// <summary>
            /// The log level of the message.
            /// </summary>
            public LogLevel Level { get; }

            /// <summary>
            /// The text of the message, after any parameters have been substituted.
            /// </summary>
            public string Text { get; }

            /// <summary>
            /// Constructs a new instance.
            /// </summary>
            /// <param name="loggerName">the name of the logger that produced the message</param>
            /// <param name="level">the log level of the message</param>
            /// <param name="text">the text of the message, after any parameters have been substituted</param>
            public Message(string loggerName, LogLevel level, string text)
            {
                LoggerName = loggerName;
                Level = level;
                Text = text;
            }

            /// <summary>
            /// Summarizes the message in the format "[LoggerName] LEVEL: text".
            /// </summary>
            /// <returns>a descriptive string</returns>
            public override string ToString() =>
                string.IsNullOrEmpty(LoggerName) ?
                    string.Format("{0}: {1}", Level.Uppercase(), Text) :
                    string.Format("[{0}] {1}: {2}", LoggerName, Level.Uppercase(), Text);
        }

        private class ChannelImpl : IChannel
        {
            private readonly List<Message> _messages;
            private readonly string _name;

            public ChannelImpl(List<Message> messages, string name)
            {
                _messages = messages;
                _name = name;
            }

            public bool IsEnabled(LogLevel level) => true;

            public void Log(LogLevel level, object message)
            {
                AddMessage(level, message.ToString());
            }

            public void Log(LogLevel level, string format, object param)
            {
                AddMessage(level, string.Format(format, param));
            }

            public void Log(LogLevel level, string format, object param1, object param2)
            {
                AddMessage(level, string.Format(format, param1, param2));
            }

            public void Log(LogLevel level, string format, params object[] allParams)
            {
                AddMessage(level, string.Format(format, allParams));
            }

            private void AddMessage(LogLevel level, string message)
            {
                lock (_messages)
                {
                    _messages.Add(new Message(_name, level, message));
                }
            }
        }
    }
}
