using System;
using System.Collections.Generic;
using System.Linq;

namespace LaunchDarkly.Logging.Tests
{
    public class BaseTest
    {
        public const string SimpleMessage = "m0";
        public static readonly object MessageParam1 = "xxx";
        public static readonly object MessageParam2 = 567;
        public static readonly object MessageParam3 = true;
        public const string MessageFormat1 = "m1:1={0}.";
        public const string MessageFormat1Result = "m1:1=xxx.";
        public const string MessageFormat2 = "m2:1={0},2={1}.";
        public const string MessageFormat2Result = "m2:1=xxx,2=567.";
        public const string MessageFormat3 = "m3:1={0},2={1},3={2}.";
        public const string MessageFormat3Result = "m3:1=xxx,2=567,3=True.";

        public static IEnumerable<LogLevel> AllLevels() =>
            new LogLevel[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Info };

        // generates parameter lists for parameterized tests
        public static IEnumerable<object[]> AllLevelsData() =>
            AllLevels().Select(level => new object[] { level });

        public static IEnumerable<object[]> AllLevelPermutationsData()
        {
            foreach (var outputLevel in AllLevels())
            {
                foreach (var enableLevel in AllLevels())
                {
                    yield return new object[] { outputLevel, enableLevel };
                }
                yield return new object[] { outputLevel, LogLevel.None };
            }
        }

        public static void WriteTestMessages(Logger logger, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(SimpleMessage);
                    logger.Debug(MessageFormat1, MessageParam1);
                    logger.Debug(MessageFormat2, MessageParam1, MessageParam2);
                    logger.Debug(MessageFormat3, MessageParam1, MessageParam2, MessageParam3);
                    break;

                case LogLevel.Info:
                    logger.Info(SimpleMessage);
                    logger.Info(MessageFormat1, MessageParam1);
                    logger.Info(MessageFormat2, MessageParam1, MessageParam2);
                    logger.Info(MessageFormat3, MessageParam1, MessageParam2, MessageParam3);
                    break;

                case LogLevel.Warn:
                    logger.Warn(SimpleMessage);
                    logger.Warn(MessageFormat1, MessageParam1);
                    logger.Warn(MessageFormat2, MessageParam1, MessageParam2);
                    logger.Warn(MessageFormat3, MessageParam1, MessageParam2, MessageParam3);
                    break;

                case LogLevel.Error:
                    logger.Error(SimpleMessage);
                    logger.Error(MessageFormat1, MessageParam1);
                    logger.Error(MessageFormat2, MessageParam1, MessageParam2);
                    logger.Error(MessageFormat3, MessageParam1, MessageParam2, MessageParam3);
                    break;
            }
        }
    }
}
