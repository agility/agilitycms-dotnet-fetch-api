# Changelog

This project follows [Semantic Versioning](https://semver.org/).

## [3.2.0-beta.1]

### Added

- GEO / AEO properties on pages: `PageResponse.Geo`, a `GeoProperties` with `SchemaType`, `Summary`,
  `StructuredData` (the raw JSON-LD object or array, as a `JsonElement?`) and `Ai` (a `GeoAi` with `LlmsInclude` and
  `Crawl`). Four new `SEO` properties: `Title`, `CanonicalUrl`, `Robots` (a `SeoRobots` with `Index` and `Follow`) and
  `OgImage`. Each is `null` when the API doesn't send it, which is the case until the Fetch API's GEO release and for
  any page without the data.
- `Geo` on content items (`ContentItem`, `GenericContentItem<T>`, so typed content lists, and
  `ContentItemResponse<T>`, so typed items and `GetContentByGraphQL`), for items in a list that backs a dynamic page.
  `null` when the item has none. `StructuredData` reads correctly through the GraphQL client, which uses
  Newtonsoft.Json.

### Fixed

- `DynamicHelpers.DeserializeSyncItemsTo<T>` now copies each item's `SEO` (and the new `Geo`); it used to drop them.
- `DynamicHelpers.DeserializeSyncItemsTo<T>` returned `null` when `T` was a type the SDK can't see, such as a private
  or internal class.

## [3.1.0] - 2026-09-30

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

### Changed

- `LICENSE` now holds the MIT text, matching the package metadata (`PackageLicenseExpression: MIT`) and the README.
  It previously contained the GPL-3.0 text by mistake.

### Removed

- A stale copy of `RedirectUrlConverter.cs` at the repository root (the package never included it).

## [3.0.2] - 2026-04-07

Published from commit `b89785e` (same code as 3.0.1).

## [3.0.1] - 2026-04-02

- `SitemapPage` redirect field type matches the API response (PROD-942).

## [3.0.0] - 2026-01-20

- Targets .NET 10; `IsPreview` on each call; GraphQL content items; single assembly.
