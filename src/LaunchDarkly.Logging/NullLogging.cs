
namespace LaunchDarkly.Logging
{
    internal class NullLogging : ILogAdapter
    {
        public static NullLogging Instance = new NullLogging();

        public IChannel NewChannel(string name)
        {
            return ChannelImpl.Instance;
        }

        private class ChannelImpl : IChannel
        {
            public static ChannelImpl Instance = new ChannelImpl();

            public bool IsEnabled(LogLevel level) => false;

            public void Log(LogLevel level, object message) { }

            public void Log(LogLevel level, string format, object param) { }

            public void Log(LogLevel level, string format, object param1, object param2) { }

            public void Log(LogLevel level, string format, params object[] allParams) { }
        }
    }
}
