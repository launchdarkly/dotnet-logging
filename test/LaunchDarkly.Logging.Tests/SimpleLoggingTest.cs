using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class SimpleLoggingTest : BaseTest
    {
        [Theory]
        [MemberData(nameof(AllLevelsData))]
        public void TestWriterOutput(LogLevel outputLevel)
        {
            var sw = new StringWriter();
            var logger = Logs.ToWriter(sw).DateFormat(null)
                .Logger("logname");
            WriteTestMessages(logger, outputLevel);
            var resultLines = ParseOutputLines(sw.ToString());
            VerifyLines(resultLines, outputLevel);
        }

        [Theory]
        [MemberData(nameof(AllLevelsData))]
        public void TestMethodOutput(LogLevel outputLevel)
        {
            var resultLines = new List<string>();
            var logger = Logs.ToMethod(resultLines.Add).DateFormat(null)
                .Logger("logname");
            WriteTestMessages(logger, outputLevel);
            VerifyLines(resultLines.ToArray(), outputLevel);
        }

        private void VerifyLines(string[] resultLines, LogLevel outputLevel)
        {
            var prefix = "[logname] " + outputLevel.Uppercase() + ": ";
            var expectedLines = new string[]
            {
                prefix + SimpleMessage,
                prefix + MessageFormat1Result,
                prefix + MessageFormat2Result,
                prefix + MessageFormat3Result
            };
            Assert.Equal(expectedLines, resultLines);
        }

        private static string[] ParseOutputLines(string s)
        {
            var s1 = s.Trim();
            if (s1 == "")
            {
                return new string[0];
            }
            return s1.Split('\n').Select(line => line.Trim()).ToArray();
        }
    }
}
