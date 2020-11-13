using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LevelFilterTest : BaseTest
    {
        [Theory]
        [MemberData(nameof(AllLevelPermutationsData))]
        public void TestOutput(LogLevel outputLevel, LogLevel enableLevel)
        {
            var capture = Logs.Capture();
            var logger = capture.Level(enableLevel).Logger("logname");
            Assert.True(logger.IsEnabled(enableLevel));
            if (enableLevel > LogLevel.Debug)
            {
                Assert.False(logger.IsEnabled(enableLevel - 1));
            }
            WriteTestMessages(logger, outputLevel);
            LogCaptureTest.VerifyCapturedOutput(outputLevel, enableLevel, "logname", capture);
        }
    }
}
