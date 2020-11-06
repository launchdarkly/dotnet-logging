// This file is not a real class but is used by Sandcastle Help File Builder to provide documentation
// for the namespace. The other way to document a namespace is to add this XML to the .shfbproj file:
//
// <NamespaceSummaries>
//   <NamespaceSummaryItem name="LaunchDarkly.Logging" isDocumented="True" xmlns="">
//     ...summary here...
//   </NamespaceSummaryItem
// </NamespaceSummaries>
//
// However, currently Sandcastle does not correctly resolve links if you use that method.

namespace LaunchDarkly.Logging
{
    /// <summary>
    /// A simple pluggable logging framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <c>LaunchDarkly.Logging</c>provides a simple logging abstraction that is used by other
    /// LaunchDarkly .NET packages, including the LaunchDarkly .NET SDK and Xamarin SDK. It includes
    /// built-in implementations for basic logging (see <see cref="LaunchDarkly.Logging.Logs"/>), and can
    /// also be connected to other logging frameworks with a simple adapter interface. It does not deal
    /// with administrative tasks such as rotating log files; the assumption is that those would be set up
    /// at an OS level or by an application framework.
    /// </para>
    /// <para>
    /// The reason for this indirect approach to logging is that LaunchDarkly tools can run on a variety
    /// of .NET platforms, including .NET Core, .NET Framework, and Xamarin. There is no single logging
    /// framework that is consistently favored across all of those. For instance, the standard in .NET
    /// Core is now <c>Microsoft.Extensions.Logging</c>, but in .NET Framework 4.5.x this is not available
    /// without bringing in .NET Core assemblies that are normally not used in .NET Framework.
    /// </para>
    /// <para>
    /// Earlier versions of LaunchDarkly SDKs used the <c>Common.Logging</c> framework, which provides
    /// adapters to various popular loggers. But writing the LaunchDarkly packages against such a
    /// third-party API causes inconvenience for any developer using LaunchDarkly who prefers a different
    /// framework, and it is a relatively heavyweight solution for projects that may have only simple logging
    /// requirements. This package, with its small feature set geared toward the needs of LaunchDarkly
    /// SDKs, aims to make the task of writing and maintaining logging adapters very straightforward, and
    /// to reduce the chance that a change in third-party APIs will cause backward incompatibillity.
    /// </para>
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    {
    }
}
