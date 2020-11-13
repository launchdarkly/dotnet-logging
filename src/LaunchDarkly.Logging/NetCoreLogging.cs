#if NETCOREAPP
using System;
using Microsoft.Extensions.Logging;

namespace LaunchDarkly.Logging
{
    internal class NetCoreLogging : ILogAdapter
    {
        private readonly ILoggerFactory _factory;

        internal NetCoreLogging(ILoggerFactory factory)
        {
            _factory = factory;
        }

        public IChannel NewChannel(string name) =>
            new ChannelImpl(_factory.CreateLogger(name));
   
        private class ChannelImpl : IChannel
        {
            private readonly ILogger _logger;

            internal ChannelImpl(ILogger logger)
            {
                _logger = logger;
            }

            private static Microsoft.Extensions.Logging.LogLevel Level(LogLevel level)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        return Microsoft.Extensions.Logging.LogLevel.Debug;
                    case LogLevel.Info:
                        return Microsoft.Extensions.Logging.LogLevel.Information;
                    case LogLevel.Warn:
                        return Microsoft.Extensions.Logging.LogLevel.Warning;
                    case LogLevel.Error:
                        return Microsoft.Extensions.Logging.LogLevel.Error;
                    default:
                        return Microsoft.Extensions.Logging.LogLevel.None;
                }
            }

            public bool IsEnabled(LogLevel level) => _logger.IsEnabled(Level(level));

            public void Log(LogLevel level, object message)
            {
                var msLevel = Level(level);
                if (_logger.IsEnabled(msLevel))
                {
                    _logger.Log(msLevel, message.ToString(), null);
                }
            }

            public void Log(LogLevel level, string format, object param)
            {
                var msLevel = Level(level);
                if (_logger.IsEnabled(msLevel))
                {
                    _logger.Log(msLevel, format, param);
                }
            }

            public void Log(LogLevel level, string format, object param1, object param2)
            {
                var msLevel = Level(level);
                if (_logger.IsEnabled(msLevel))
                {
                    _logger.Log(msLevel, format, param1, param2);
                }
            }

            public void Log(LogLevel level, string format, params object[] allParams)
            {
                var msLevel = Level(level);
                if (_logger.IsEnabled(msLevel))
                {
                    _logger.Log(msLevel, format, allParams);
                }
            }
        }
    }
}
#endif
