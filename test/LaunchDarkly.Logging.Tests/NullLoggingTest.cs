using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class NullLoggingTest : BaseTest
    {
        // Just verifies that writing to the null logger doesn't throw an exception.

        [Theory]
        [MemberData(nameof(AllLevelsData))]
        public void TestNullLogging(LogLevel outputLevel)
        {
            var logger = Logs.None.Logger("logname");
            WriteTestMessages(logger, outputLevel);
        }
    }
}
