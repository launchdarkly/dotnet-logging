# LaunchDarkly Logging

[![CircleCI](https://circleci.com/gh/launchdarkly/dotnet-logging/tree/master.svg?style=svg)](https://circleci.com/gh/launchdarkly/dotnet-logging/tree/master)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Flaunchdarkly%2Fdotnet-logging.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Flaunchdarkly%2Fdotnet-logging?ref=badge_shield)

This .NET package provides a basic logging abstraction that is used by other LaunchDarkly .NET packages, including the LaunchDarkly [.NET SDK](https://github.com/launchdarkly/dotnet-server-sdk) and [Xamarin SDK](https://github.com/launchdarkly/xamarin-client-sdk). It can be connected to other logging frameworks with a simple adapter interface.

The reason for this indirect approach to logging is that LaunchDarkly tools can run on a variety of .NET platforms, including .NET Core, .NET Framework, and Xamarin. There is no single logging framework that is consistently favored across all of those. For instance, the standard in .NET Core is now `Microsoft.Extensions.Logging`, but in .NET Framework 4.5.x this is not available without bringing in .NET Core assemblies that are normally not used in .NET Framework.

Earlier versions of LaunchDarkly SDKs used the [`Common.Logging`](https://github.com/net-commons/common-logging) framework, which provides adapters to various popular loggers. But writing the LaunchDarkly packages against such a third-party API causes inconvenience for any developer using LaunchDarkly who prefers a different framework, and it is a relatively heavyweight solution for projects that may only have simple logging requirements. This package, with its small feature set geared toward the needs of LaunchDarkly SDKs, aims to make the task of writing and maintaining logging adapters very straightforward, and to reduce the chance that a change in third-party APIs will cause backward incompatibillity.

Here are simple examples of configuring the LaunchDarkly server-side SDK for .NET to use some of the standard logging implementations. For more examples of how to specify a logging implementation when using the LaunchDarkly SDKs or other libraries, consult the documentation for those libraries. Each library may have its own rules for what the default logging implementation is if you don't specify one.

```csharp
    // In this configuration, logging goes to the standard output stream (Console.Out).
    var config1 = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.ToStream(Console.Out)))
        .Build();

    // Same, but all logging below Warn level is suppressed.
    var config2 = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.ToStream(Console.Out).Level(LogLevel.Warn)))
        .Build();

    // This configuration disables all logging.
    var config3 = Configuration.Builder("my-sdk-key")
        .Logging(Components.Logging(Logs.None))
        .Build();
```


## Contributing

See [Contributing](https://github.com/launchdarkly/dotnet-logging/blob/master/CONTRIBUTING.md).

## Signing

The published version of this assembly is digitally signed with Authenticode, and also strong-named. The public key file is in this repo at `LaunchDarkly.Logging.pk` as well as here:

```
Public Key:
0024000004800000940000000602000000240000525341310004000001000100
250509411af6d31f2abfc9b33d02b01c6ad14fd5c7f83cc6135f499ebb0ec8f3
4e05c59e49232f5a7d75d5761281610219d323043936d55c19bb26f1dd86bdc7
6ab178015e78b54aef9cbdc824db2afcf7250292ae3d8d9c4522bcc3a4fc4831
d4b4320e820f32e024ad50a786f86d37ea45e0c25ec431a7a0f3e93575a0d2ad

Public Key Token: 45ef1738a929a7df
```

Building the code locally in the default Debug configuration does not sign the assembly and does not require a key file. Note that the unit tests can only be run in the Debug configuration.

## About LaunchDarkly
 
* LaunchDarkly is a continuous delivery platform that provides feature flags as a service and allows developers to iterate quickly and safely. We allow you to easily flag your features and manage them from the LaunchDarkly dashboard.  With LaunchDarkly, you can:
    * Roll out a new feature to a subset of your users (like a group of users who opt-in to a beta tester group), gathering feedback and bug reports from real-world use cases.
    * Gradually roll out a feature to an increasing percentage of users, and track the effect that the feature has on key metrics (for instance, how likely is a user to complete a purchase if they have feature A versus feature B?).
    * Turn off a feature that you realize is causing performance problems in production, without needing to re-deploy, or even restart the application with a changed configuration file.
    * Grant access to certain features based on user attributes, like payment plan (eg: users on the ‘gold’ plan get access to more features than users in the ‘silver’ plan). Disable parts of your application to facilitate maintenance, without taking everything offline.
* LaunchDarkly provides feature flag SDKs for a wide variety of languages and technologies. Check out [our documentation](https://docs.launchdarkly.com/docs) for a complete list.
* Explore LaunchDarkly
    * [launchdarkly.com](https://www.launchdarkly.com/ "LaunchDarkly Main Website") for more information
    * [docs.launchdarkly.com](https://docs.launchdarkly.com/  "LaunchDarkly Documentation") for our documentation and SDK reference guides
    * [apidocs.launchdarkly.com](https://apidocs.launchdarkly.com/  "LaunchDarkly API Documentation") for our API documentation
    * [blog.launchdarkly.com](https://blog.launchdarkly.com/  "LaunchDarkly Blog Documentation") for the latest product updates
    * [Feature Flagging Guide](https://github.com/launchdarkly/featureflags/  "Feature Flagging Guide") for best practices and strategies
