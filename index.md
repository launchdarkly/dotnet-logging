The [`LaunchDarkly.Logging`](https://nuget.org/packages/LaunchDarkly.Logging) package provides a simple logging abstraction that is used by other LaunchDarkly .NET packages, including the [LaunchDarkly server-side .NET SDK](https://github.com/launchdarkly/dotnet-server-sdk) and [Xamarin SDK](https://github.com/launchdarkly/xamarin-client-sdk). It does not deal with administrative tasks such as rotating log files; the assumption is that those would be set up at an OS level or by an application framework.

For a complete list of types and methods, see the [API documentation](xref:LaunchDarkly.Logging).

There are built-in implementations for basic logging: see <xref:LaunchDarkly.Logging.Logs>. The API can also be connected to other logging frameworks with a simple adapter interface, and LaunchDarkly provides several such adapters; see "Adapters" below.

For source code, see the [GitHub repository](https://github.com/launchdarkly/dotnet-logging).

## Rationale

The reason for this indirect approach to logging is that LaunchDarkly tools can run on a variety of .NET platforms, including .NET Core, .NET Framework, .NET 5, and Xamarin. There is no single logging framework that is consistently favored across all of those. For instance, the standard in .NET Core is now `Microsoft.Extensions.Logging`, but in .NET Framework 4.5.x this is not available without bringing in .NET Core assemblies that are normally not used in .NET Framework.

Earlier versions of LaunchDarkly SDKs used the [`Common.Logging`](https://github.com/net-commons/common-logging) framework, which provides adapters to various popular loggers. But writing the LaunchDarkly packages against such a third-party API causes inconvenience for any developer using LaunchDarkly who prefers a different framework, and it is a relatively heavyweight solution for projects that may have only simple logging requirements. This package, with its small feature set geared toward the needs of LaunchDarkly SDKs, aims to make the task of writing and maintaining logging adapters very straightforward, and to reduce the chance that a change in third-party APIs will cause backward incompatibillity.

## Examples

The example code below shows how to configure the LaunchDarkly server-side SDK for .NET to use some of the standard logging implementations. For more examples of how to specify a logging implementation when using the LaunchDarkly SDKs or other libraries, consult the documentation for those libraries. Each library may have its own rules for what the default logging implementation is if you don't specify one.

In this configuration, logging goes to the standard output stream (Console.Out):

```csharp
    var config = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.ToStream(Console.Out)))
        .Build();
```

This is the same, except all logging below Warn level is suppressed:

```csharp
    var config = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.ToStream(Console.Out).Level(LogLevel.Warn)))
        .Build();
```

This configuration disables all logging (the SDK has a shortcut for doing the same thing, but the example shows how you can use `Logs.None` anywhere that a log adapter is expected):

```csharp
    var config = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.None))
        .Build();
```

## Adapters

If you want to send logging to a destination that isn't built into this package, the `LaunchDarkly.Logging` API allows you to define your own adapter by implementing the `ILogAdapter` interface. We have already created implementations for use with several popular logging frameworks:

* [Common.Logging](https://launchdarkly.github.io/dotnet-logging-adapter-commonlogging)
* [Log4net](https://launchdarkly.github.io/dotnet-logging-adapter-log4net)
* [Microsoft.Extensions.Logging](https://launchdarkly.github.io/dotnet-logging-adapter-ms)
* [NLog](https://launchdarkly.github.io/dotnet-logging-adapter-nlog)
