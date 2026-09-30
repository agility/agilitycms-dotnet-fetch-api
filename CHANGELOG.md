# Changelog

This project follows [Semantic Versioning](https://semver.org/).

## [3.1.0] - Unreleased

### Fixed

- `GetContentByGraphQL` queried the `en-us` endpoint whatever locale was passed. It now uses the requested locale.
- The GraphQL client cache is safe to use from concurrent requests.

### Added

- Error messages for non-200 responses include the API's response and the request path, after the unchanged
  `HttpException: {status} - {reason}` prefix.
- Every request sends `X-Agility-SDK: agility-fetch-sdk-dotnet/<version>`, also used as the User-Agent.
- GraphQL requests go through the service's `HttpClient`, so handlers configured with `AddHttpClient` apply to them.
- Source Link and a symbol package.
- Releases publish from CI on a version tag, using NuGet Trusted Publishing.

### Removed

- A stale copy of `RedirectUrlConverter.cs` at the repository root (the package never included it).

## [3.0.2] - 2026-04-07

Published from commit `b89785e` (same code as 3.0.1).

## [3.0.1] - 2026-04-02

- `SitemapPage` redirect field type matches the API response (PROD-942).

## [3.0.0] - 2026-01-20

- Targets .NET 10; `IsPreview` on each call; GraphQL content items; single assembly.
