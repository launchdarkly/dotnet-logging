using System;

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Helper methods for logging special variables.
    /// </summary>
    public static class LogValues
    {
        /// <summary>
        /// Converts any function that returns a string into an object for calling it lazily.
        /// </summary>
        /// <remarks>
        /// Sometimes log messages may include a computed value that has enough computational
        /// overhead that you would prefer not to compute it unless it really will be logged.
        /// The <see cref="Logger"/> methods that take parameters of type <c>object</c> do not
        /// call <c>ToString()</c> to convert those parameters to strings immediately; if logging
        /// of this message has been disabled by a <see cref="ILogAdapterExtensions.Level(ILogAdapter, LogLevel)"/>
        /// filter, or some other filtering mechanism defined by the log adapter, or if all
        /// logging is disabled because the destination is <see cref="Logs.None"/>, then
        /// <c>ToString()</c> is not called. The object returned by <c>Defer</c> simply delegates
        /// its <c>ToString()</c> method to the function you provide.
        /// </remarks>
        /// <example>
        ///     // Here, ComputeJSONData is only called if debug-level logging is enabled
        ///     logger.Debug("The JSON data is: {0}", () => ComputeJSONData());
        /// </example>
        /// <param name="stringProvider">a function that returns a string</param>
        /// <returns>an object that calls that function if <c>ToString()</c> is called</returns>
        public static object Defer(Func<string> stringProvider) =>
            new DeferImpl(stringProvider);

        /// <summary>
        /// Returns an object that lazily constructs a basic exception description.
        /// </summary>
        /// <remarks>
        /// Calling <c>ToString()</c> on the object returned by this method returns a string
        /// that includes the name of the exception class, its Message (if any), and the
        /// same properties for its InnerException (if any). This string is not constructed
        /// unless <c>ToString()</c> is called, so writing exceptions to the log in this way
        /// incurs very little overhead if logging is not enabled for the specified log level.
        /// </remarks>
        /// <example>
        ///     try { ... }
        ///     catch (Exception e)
        ///     {
        ///         logger.Debug("Caught: {0} {1}", LogValues.ExceptionSummary(e),
        ///             LogValues.ExceptionTrace(e));
        ///     }
        /// </example>
        /// <param name="e">an exception</param>
        /// <returns>an object whose <c>ToString()</c> method provides a description</returns>
        public static object ExceptionSummary(Exception e) =>
            Defer(() =>
                e.InnerException is null ? DescribeException(e) :
                    string.Format("{0} (caused by: {1})", DescribeException(e),
                        DescribeException(e.InnerException)));

        private static string DescribeException(Exception e) =>
            string.IsNullOrEmpty(e.Message) ? e.GetType().ToString() :
                string.Format("{0}: {1}", e.GetType(), e.Message);

        /// <summary>
        /// Returns an object that lazily constructs an exception stacktrace.
        /// </summary>
        /// <remarks>
        /// Calling <c>ToString()</c> on the object returned by this method returns the
        /// exception's stacktrace as a string. This string is not constructed unless
        /// <c>ToString()</c> is called, so writing exceptions to the log in this way incurs
        /// very little overhead if logging is not enabled for the specified log level.
        /// </remarks>
        /// <example>
        ///     try { ... }
        ///     catch (Exception e)
        ///     {
        ///         logger.Debug("Caught: {0} {1}", LogValues.ExceptionSummary(e),
        ///             LogValues.ExceptionTrace(e));
        ///     }
        /// </example>
        /// <param name="e">an exception</param>
        /// <returns>an object whose <c>ToString()</c> method provides a stacktrace</returns>
        public static object ExceptionTrace(Exception e) => Defer(() => e.StackTrace);

        private class DeferImpl
        {
            private readonly Func<string> _provider;

            internal DeferImpl(Func<string> provider)
            {
                _provider = provider;
            }

            public override string ToString() => _provider();
        }
    }
}
