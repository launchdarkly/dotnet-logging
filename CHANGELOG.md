# Change log

All notable changes to the project will be documented in this file. For full release notes for the projects that depend on this project, see their respective changelogs. This file describes changes only to the common code. This project adheres to [Semantic Versioning](http://semver.org).

## [2.1.0](https://github.com/launchdarkly/dotnet-logging/compare/2.0.0...2.1.0) (2026-09-22)


### Features

* replace EOL netcoreapp3.1 target with net8.0 ([02a5cf2](https://github.com/launchdarkly/dotnet-logging/commit/02a5cf20dff215f6fa90ca6acc82459d9c623d2a))
* replace EOL netcoreapp3.1 target with net8.0 ([#23](https://github.com/launchdarkly/dotnet-logging/issues/23)) ([4f56f24](https://github.com/launchdarkly/dotnet-logging/commit/4f56f24ac6738c2f0a8f87ce5e7485e6f94b7c1d))


### Bug Fixes

* add release-please version markers to csproj ([4f73aba](https://github.com/launchdarkly/dotnet-logging/commit/4f73aba47a51e970410096a34238c27e9514d883))
* add release-please version markers to csproj ([#21](https://github.com/launchdarkly/dotnet-logging/issues/21)) ([903271e](https://github.com/launchdarkly/dotnet-logging/commit/903271e1b2f3a48d56a26f26d201b2dafe550c97))
* match Microsoft.Extensions.Logging.Abstractions to the net8.0 framework band ([4698802](https://github.com/launchdarkly/dotnet-logging/commit/4698802e077b9dbf84b612c621962e2e011ebe1f))

## [2.0.0] - 2022-08-17
### Changed:
- Removed EOL target frameworks .NET Core 2.1 and .NET Framework 4.5.2. Lowest compatible platform versions are now .NET Core 3.1, .NET Framework 4.6.2, .NET 6.0, and .NET Standard 2.0.

## [1.0.1] - 2021-02-02
### Added:
- Source Link package is now published.

### Fixed:
- Minor corrections to project metadata and documentation.

## [1.0.0] - 2021-01-28
Initial release of this package, to be used in version 6.0 of the LaunchDarkly .NET SDK, version 2.0 of the LaunchDarkly Xamarin SDK, and related libraries.
