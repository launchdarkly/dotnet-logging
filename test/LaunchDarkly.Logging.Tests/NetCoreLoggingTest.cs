#if NETCOREAPP

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class NetCoreLoggingTest
    {
        [Fact]
        public void LoggerDelegatesToNetCoreLogger()
        {
            var fixture = new TestLoggerProvider();
            var factory = new LoggerFactory();
            factory.AddProvider(fixture);
            var logger = Logs.CoreLogging(factory).Logger("logname");
            logger.Info("hello");
            Assert.Equal(new List<string> { "[logname] Information: hello" }, fixture.messages);
        }
    }

    public class TestLoggerProvider : ILoggerProvider
    {
        public readonly List<string> messages = new List<string>();

        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger(messages, categoryName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class TestLogger : ILogger
    {
        private readonly List<string> _messages;
        private readonly string _name;

        public TestLogger(List<string> messages, string name)
        {
            _messages = messages;
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Format("[{0}] {1}: {2}", _name, logLevel, formatter(state, null));
            _messages.Add(message);
        }
    }
}

#endif
