using System.Collections.Generic;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LogCaptureTest : BaseTest
    {
        [Theory]
        [MemberData(nameof(AllLevelsData))]
        public void TestOutput(LogLevel outputLevel)
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            WriteTestMessages(logger, outputLevel);
            VerifyCapturedOutput(outputLevel, LogLevel.Debug, "logname", capture);
        }

        [Fact]
        public void TestHasMessageWithText()
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            logger.Info("first");
            logger.Info("has second as substring");
            logger.Warn("second");
            Assert.True(capture.HasMessageWithText(LogLevel.Info, "first"));
            Assert.False(capture.HasMessageWithText(LogLevel.Info, "second"));
            Assert.True(capture.HasMessageWithText(LogLevel.Warn, "second"));
        }

        [Fact]
        public void TestHasMessageWithRegex()
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            logger.Info("first");
            logger.Info("has second as substring");
            logger.Warn("second");
            Assert.True(capture.HasMessageWithRegex(LogLevel.Info, ".rs"));
            Assert.False(capture.HasMessageWithRegex(LogLevel.Info, "^second"));
            Assert.True(capture.HasMessageWithRegex(LogLevel.Warn, "^second"));
        }

        [Fact]
        public void TestToString()
        {
            var capture = Logs.Capture();
            var logger = capture.Logger("logname");
            logger.Info("first");
            logger.Info("second with {0}", "parameter");
            logger.Warn("third");
            Assert.Equal("[logname] INFO: first\n[logname] INFO: second with parameter\n[logname] WARN: third",
                capture.ToString());
        }

        internal static void VerifyCapturedOutput(LogLevel outputLevel,
            LogLevel enableLevel, string logName, LogCapture capture)
        {
            var messages = capture.GetMessages();
            var strings = capture.GetMessageStrings();
            if (enableLevel > outputLevel)
            {
                Assert.Empty(messages);
                Assert.Empty(strings);
            }
            else
            {
                var expectedMessages = new List<LogCapture.Message>
                {
                    new LogCapture.Message(logName, outputLevel, SimpleMessage),
                    new LogCapture.Message(logName, outputLevel, MessageFormat1Result),
                    new LogCapture.Message(logName, outputLevel, MessageFormat2Result),
                    new LogCapture.Message(logName, outputLevel, MessageFormat3Result)
                };
                var prefix = string.IsNullOrEmpty(logName) ?
                    outputLevel.Uppercase() + ": " :
                    "[" + logName + "] " + outputLevel.Uppercase() + ": ";
                var expectedStrings = new List<string>
                {
                    prefix + SimpleMessage,
                    prefix + MessageFormat1Result,
                    prefix + MessageFormat2Result,
                    prefix + MessageFormat3Result
                };
                Assert.Equal(expectedMessages, messages);
                Assert.Equal(expectedStrings, strings);
            }
        }
    }
}
