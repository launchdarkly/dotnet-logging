using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class MultiLoggingTest : BaseTest
    {
        [Theory]
        [MemberData(nameof(AllLevelsData))]
        public void TestOutput(LogLevel outputLevel)
        {
            var sink1 = Logs.Capture();
            var sink2 = Logs.Capture();
            var logger = Logs.ToMultiple(sink1, sink2).Logger("");
            WriteTestMessages(logger, outputLevel);
            LogCaptureTest.VerifyCapturedOutput(outputLevel, LogLevel.Debug, "", sink1);
            LogCaptureTest.VerifyCapturedOutput(outputLevel, LogLevel.Debug, "", sink2);
        }

        [Fact]
        public void LevelIsEnabledIfItIsEnabledForAnyDestinationLogger()
        {
            var infoLevelLogger = Logs.ToConsole.Level(LogLevel.Info);
            var warnLevelLogger = Logs.ToConsole.Level(LogLevel.Warn);
            var multi = Logs.ToMultiple(infoLevelLogger, warnLevelLogger).Logger("");
            Assert.True(multi.IsEnabled(LogLevel.Warn));
            Assert.True(multi.IsEnabled(LogLevel.Info));
            Assert.False(multi.IsEnabled(LogLevel.Debug));

            var empty = Logs.ToMultiple().Logger("");
            Assert.False(empty.IsEnabled(LogLevel.Info));
        }
    }
}
