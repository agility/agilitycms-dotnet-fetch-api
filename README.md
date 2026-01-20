# Agility CMS Fetch SDK for .NET

[![NuGet](https://img.shields.io/nuget/v/Agility.NET.FetchAPI.svg)](https://www.nuget.org/packages/Agility.NET.FetchAPI)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Agility.NET.FetchAPI.svg)](https://www.nuget.org/packages/Agility.NET.FetchAPI)

Official .NET SDK for pulling content from your Agility CMS instance via the Fetch API.

## What's New in 3.0

- **Upgraded to .NET 10** - Now targets `net10.0` for the latest performance and language features
- Updated all dependencies to their latest versions
- `IsPreview` is a parameter set on EACH method call
- GraphQL Content Items support
- Single unified assembly (removed `Shared` and `Core` projects)

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Agility.NET.FetchAPI
```

Or add to your `.csproj`:

```xml
<PackageReference Include="Agility.NET.FetchAPI" Version="3.0.0" />
```

## Requirements

- .NET 10.0 or later

## Quick Start

### 1. Configure Services

In your `Program.cs` or startup configuration:

```csharp
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Services;

// Configure Agility settings
builder.Services.Configure<AppSettings>(options =>
{
    options.InstanceGUID = "your-instance-guid";
    options.FetchAPIKey = "your-fetch-api-key";
    options.PreviewAPIKey = "your-preview-api-key";
    options.Locales = "en-us";
    options.ChannelName = "website";
});

// Register HttpClient and FetchApiService
builder.Services.AddHttpClient<FetchApiService>();
```

### 2. Inject and Use the Service

```csharp
public class MyController : Controller
{
    private readonly FetchApiService _agilityService;

    public MyController(FetchApiService agilityService)
    {
        _agilityService = agilityService;
    }

    public async Task<IActionResult> GetContent()
    {
        var item = await _agilityService.GetContentItem(
            new GetItemParameters
            {
                IsPreview = false,
                Locale = "en-us",
                ContentId = 123,
                ContentLinkDepth = 2
            });

        return Ok(item);
    }
}
```

## Configuration Options

```csharp
public class AppSettings
{
    public string InstanceGUID { get; set; }      // Your Agility instance GUID
    public string SecurityKey { get; set; }       // Security key (optional)
    public string WebsiteName { get; set; }       // Website name (optional)
    public string FetchAPIKey { get; set; }       // API key for fetch (live) mode
    public string PreviewAPIKey { get; set; }     // API key for preview mode
    public string Locales { get; set; }           // Supported locales (e.g., "en-us")
    public string ChannelName { get; set; }       // Default channel name
    public int CacheInMinutes { get; set; }       // Cache duration in minutes
}
```

## Regional Support

The SDK automatically routes requests to the correct regional endpoint based on your Instance GUID suffix:

| Region | GUID Suffix | Endpoint |
|--------|-------------|----------|
| US (default) | — | `https://api.aglty.io` |
| USA 2 | `-us2` | `https://api-usa2.aglty.io` |
| Canada | `-c` | `https://api-ca.aglty.io` |
| Europe | `-e` | `https://api-eu.aglty.io` |
| Australia | `-a` | `https://api-aus.aglty.io` |
| Development | `-d` | `https://api-dev.aglty.io` |

## Fetch API Methods

| Function | Parameters | Description |
|:---------|:-----------|:------------|
| `GetContentItem` | `GetItemParameters` | Get a single content item |
| `GetContentList` | `GetListParameters` | Get a content list |
| `GetGallery` | `GetGalleryParameters` | Get a gallery |
| `GetPage` | `GetPageParameters` | Get a page |
| `GetSitemapFlat` | `GetSitemapParameters` | Get a flat sitemap |
| `GetSitemapNested` | `GetSitemapParameters` | Get a nested sitemap |
| `GetUrlRedirections` | `GetUrlRedirectionsParameters` | Get URL redirections |
| `GetSyncContent` | `GetSyncParameters` | Sync all content using a sync token |
| `GetSyncPages` | `GetSyncParameters` | Sync all pages using a sync token |
| `GetContentByGraphQL` | GraphQL query | Query content using GraphQL |

### Typed Methods

The SDK also provides typed/generic versions of the main methods for strongly-typed responses:

- `GetTypedContentItem<T>`
- `GetTypedContentList<T>`
- `GetTypedPage<T>`
- `GetTypedSitemapFlat<T>`
- `GetTypedSitemapNested<T>`

## Parameter Models

### GetItemParameters

```csharp
public class GetItemParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public string Locale { get; set; }
    public int ContentId { get; set; }
    public int ContentLinkDepth { get; set; }
    public bool ExpandAllContentLinks { get; set; }
}
```

### GetListParameters

```csharp
public class GetListParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public string Locale { get; set; }
    public string ReferenceName { get; set; }
    public string Fields { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
    public string Filter { get; set; }
    public string Sort { get; set; }
    public string Direction { get; set; }
    public int ContentLinkDepth { get; set; }
    public bool ExpandAllContentLinks { get; set; }
}
```

### GetGalleryParameters

```csharp
public class GetGalleryParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public int GalleryId { get; set; }
}
```

### GetPageParameters

```csharp
public class GetPageParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public string Locale { get; set; }
    public int PageId { get; set; }
    public int ContentLinkDepth { get; set; }     // Must be 0 for GetTypedPage
    public bool ExpandAllContentLinks { get; set; }
}
```

### GetSitemapParameters

```csharp
public class GetSitemapParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public string Locale { get; set; }
    public string ChannelName { get; set; }
}
```

### GetUrlRedirectionsParameters

```csharp
public class GetUrlRedirectionsParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public DateTime? LastAccessDate { get; set; }
}
```

### GetSyncParameters

```csharp
public class GetSyncParameters
{
    public bool IsPreview { get; set; }           // Use preview or fetch mode
    public string Locale { get; set; }
    public long SyncToken { get; set; }
    public int PageSize { get; set; }
}
```

## GraphQL Support

Query content using GraphQL for more flexible data retrieval:

```csharp
var query = @"
{
    posts {
        contentID
        fields {
            title
            content
        }
    }
}";

var result = await _agilityService.GetContentByGraphQL(query, isPreview: false);
```

## Development Setup

1. Clone the repo:
   ```bash
   git clone https://github.com/agility/agilitycms-dotnet-fetch-api
   ```

2. Open in your IDE and restore packages:
   ```bash
   dotnet restore
   ```

3. Build:
   ```bash
   dotnet build
   ```

4. Configure test credentials:
   ```bash
   cp tests/Agility.NET.FetchAPI.Tests/test.runsettings.template tests/Agility.NET.FetchAPI.Tests/test.runsettings
   ```
   Edit `test.runsettings` with your Agility credentials.

5. Run tests:
   ```bash
   dotnet test --settings tests/Agility.NET.FetchAPI.Tests/test.runsettings
   ```

## CI/CD

This project uses GitHub Actions for continuous integration. To run tests in CI, configure the following repository secrets:

| Secret | Description |
|--------|-------------|
| `AGILITY_INSTANCE_GUID` | Your Agility CMS instance GUID |
| `AGILITY_FETCH_API_KEY` | API key for fetch (live) mode |
| `AGILITY_PREVIEW_API_KEY` | API key for preview mode |

## Integration with Agility .NET Starter

To use this SDK with the [Agility CMS .NET Starter](https://github.com/agility/agilitycms-dotnet-starter):

1. Add a project reference or install the NuGet package
2. Configure `AppSettings` with your Agility credentials
3. Register `FetchApiService` with dependency injection

## License

MIT License - see [LICENSE](LICENSE) for details.

## Resources

- [Agility CMS Documentation](https://agilitycms.com/docs)
- [Agility CMS .NET Starter](https://github.com/agility/agilitycms-dotnet-starter)
- [NuGet Package](https://www.nuget.org/packages/Agility.NET.FetchAPI)
