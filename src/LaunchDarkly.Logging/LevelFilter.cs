using System;
namespace LaunchDarkly.Logging
{
    internal class LevelFilter : ILogAdapter
    {
        private readonly ILogAdapter _wrappedAdapter;
        private readonly LogLevel _enableLevel;

        internal LevelFilter(ILogAdapter wrappedAdapter, LogLevel enableLevel)
        {
            _wrappedAdapter = wrappedAdapter;
            _enableLevel = enableLevel;
        }

        /// <summary>
        /// Called internally by the logging framework.
        /// </summary>
        /// <param name="name">the channel name</param>
        /// <returns>a new channel</returns>
        public IChannel NewChannel(String name) =>
            new ChannelImpl(_wrappedAdapter.NewChannel(name), _enableLevel);

        private class ChannelImpl : IChannel
        {
            private readonly IChannel _wrappedChannel;
            private readonly LogLevel _enableLevel;

            internal ChannelImpl(IChannel wrappedChannel, LogLevel enableLevel)
            {
                _wrappedChannel = wrappedChannel;
                _enableLevel = enableLevel;
            }

            public bool IsEnabled(LogLevel level) =>
                _enableLevel <= level && _wrappedChannel.IsEnabled(level);

            public void Log(LogLevel level, object message)
            {
                if (_enableLevel <= level)
                {
                    _wrappedChannel.Log(level, message);
                }
            }

            public void Log(LogLevel level, string format, object param)
            {
                if (_enableLevel <= level)
                {
                    _wrappedChannel.Log(level, format, param);
                }
            }

            public void Log(LogLevel level, string format, object param1, object param2)
            {
                if (_enableLevel <= level)
                {
                    _wrappedChannel.Log(level, format, param1, param2);
                }
            }

            public void Log(LogLevel level, string format, params object[] allParams)
            {
                if (_enableLevel <= level)
                {
                    _wrappedChannel.Log(level, format, allParams);
                }
            }
        }
    }
}
