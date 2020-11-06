using System;

namespace LaunchDarkly.Logging
{
    internal class MultiLogging : ILogAdapter
    {
        private readonly ILogAdapter[] _destinations;

        internal MultiLogging(ILogAdapter[] destinations)
        {
            _destinations = new ILogAdapter[destinations.Length];
            Array.Copy(destinations, _destinations, destinations.Length);
        }

        public IChannel NewChannel(string name)
        {
            var channels = new IChannel[_destinations.Length];
            for (var i = 0; i < _destinations.Length; i++)
            {
                channels[i] = _destinations[i].NewChannel(name);
            }
            return new ChannelImpl(channels);
        }

        private class ChannelImpl : IChannel
        {
            private readonly IChannel[] _channels;

            internal ChannelImpl(IChannel[] channels)
            {
                _channels = channels;
            }

            public bool IsEnabled(LogLevel level)
            {
                if (_channels.Length == 0)
                {
                    return false;
                }
                foreach (var c in _channels)
                {
                    if (c.IsEnabled(level))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void Log(LogLevel level, object message)
            {
                foreach (var c in _channels) { c.Log(level, message); }
            }

            public void Log(LogLevel level, string format, object param)
            {
                foreach (var c in _channels) { c.Log(level, format, param); }
            }

            public void Log(LogLevel level, string format, object param1, object param2)
            {
                foreach (var c in _channels) { c.Log(level, format, param1, param2); }
            }

            public void Log(LogLevel level, string format, params object[] allParams)
            {
                foreach (var c in _channels) { c.Log(level, format, allParams); }
            }
        }
    }
}
