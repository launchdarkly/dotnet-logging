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
    }
}
