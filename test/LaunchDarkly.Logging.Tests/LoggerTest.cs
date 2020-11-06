using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LoggerTest
    {
        [Fact]
        public void CanCreateRootLogger()
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            logger.Debug("hello");
            Assert.Single(capture.GetMessages());
            Assert.Equal("logname", capture.GetMessages()[0].LoggerName);
        }

        [Fact]
        public void CanCreateSubLogger()
        {
            var capture = Logs.Capture();
            var logger1 = capture.Logger("logname");
            var logger2 = logger1.SubLogger("other");
            logger2.Debug("hello");
            Assert.Single(capture.GetMessages());
            Assert.Equal("logname.other", capture.GetMessages()[0].LoggerName);
        }

        [Fact]
        public void SubLoggerWithNoNameIsSameAsParentLogger()
        {
            var logger = Logs.ToConsole.Logger("logname");
            var logger1 = logger.SubLogger(null);
            var logger2 = logger.SubLogger("");
            Assert.Same(logger, logger1);
            Assert.Same(logger, logger2);
        }

        [Fact]
        public void NullsAreIgnored()
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            logger.Debug(null);
            logger.Debug(null, "a");
            logger.Debug(null, "a", "b");
            logger.Debug(null, "a", "b", "c");
            logger.Info(null);
            logger.Info(null, "a");
            logger.Info(null, "a", "b");
            logger.Info(null, "a", "b", "c");
            logger.Warn(null);
            logger.Warn(null, "a");
            logger.Warn(null, "a", "b");
            logger.Warn(null, "a", "b", "c");
            logger.Error(null);
            logger.Error(null, "a");
            logger.Error(null, "a", "b");
            logger.Error(null, "a", "b", "c");
            Assert.Empty(capture.GetMessages());
        }
    }
}
