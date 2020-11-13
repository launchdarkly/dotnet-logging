using System;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LogValuesTest
    {
        [Fact]
        public void DeferCallsFunctionOnlyWhenStringified()
        {
            var calls = 0;
            var thing = LogValues.Defer(() =>
            {
                calls++;
                return String.Format("calls={0}", calls);
            });
            Assert.Equal(0, calls);
            var s1 = thing.ToString();
            Assert.Equal("calls=1", s1);
            Assert.Equal(1, calls);
            var s2 = thing.ToString();
            Assert.Equal("calls=2", s2);
        }

        [Fact]
        public void ExceptionSummary()
        {
            var e2 = new ArgumentException("bad");
            Assert.Equal("System.ArgumentException: bad",
                LogValues.ExceptionSummary(e2).ToString());

            var e3 = new ArgumentException("bad", new NotSupportedException("worse"));
            Assert.Equal("System.ArgumentException: bad (caused by: System.NotSupportedException: worse)",
                LogValues.ExceptionSummary(e3).ToString());
        }

        [Fact]
        public void ExceptionTrace()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                var s = LogValues.ExceptionTrace(e).ToString();
                Assert.Contains("LogValuesTest.ExceptionTrace", s);
            }
        }
    }
}
