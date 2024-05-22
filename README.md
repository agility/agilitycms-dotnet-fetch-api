# Agility CMS Fetch SDK for .Net

Agility package to pull content from Agility instance via Fetch API.

**Note:** Agility CMS .NET Fetch API does not include page management or URL redirections - this functionality exists in Agility CMS .NET Core repo https://github.com/agility/agilitycms-dotnet-core

# Setup

1. Clone Agility CMS .NET Fetch API repo at https://github.com/agility/agilitycms-dotnet-fetch-api
2. Clone and configure Agility CMS .NET Starter at https://github.com/agility/agilitycms-dotnet-starter
3. Open Agility CMS .NET Starter and add the project Agility CMS .NET Fetch API to your solution

4. Finally add a dependency by right clicking 'Dependencies' under Agility.NET.Starter

5. Select 'Add Project Reference' and check Agility.NET.FetchAPI

# Fetch API

The Fetch API supports the following calls.
Function | Parameters | Description
:--- | :--- | :---
GetContentItem | `GetItemParameters` | Get a single content item
GetContentList | `GetListParameters` | Get a content list
GetGallery | `GetGalleryParameters` | Get a gallery
GetPage | `GetPageParameters` | Get a page
GetSitemapFlat | `GetSitemapParameters` | Get a flat sitemap
GetSitemapNested | `GetSitemapParameters` | Get a nested sitemap
GetUrlRedirections | `GetUrlRedirectionsParameters` | Get URL redirections
GetSyncContent | `GetSyncParameters` | Grab all content using a sync token
GetSyncPages | `GetSyncParameters` | Get all pages using a sync token

## Parameter Models

```
    public class GetItemParameters
    {
        public string Locale { get; set; }
        public int ContentId { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
```

```
    public class GetListParameters
    {
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

```
    public class GetGalleryParameters
    {
        public int GalleryId { get; set; }
    }
```

```
    public class GetPageParameters
    {
        public string Locale { get; set; }
        public int PageId { get; set; }
        public int ContentLinkDepth { get; set; }
        public bool ExpandAllContentLinks { get; set; }
    }
```

```
    public class GetSitemapParameters
    {
        public string Locale { get; set; }
        public string ChannelName { get; set; }
    }
```

```
    public class GetUrlRedirectionsParameters
    {
        public DateTime? LastAccessDate { get; set; }
    }
```

```
    public class GetSyncParameters
    {
        public string Locale { get; set; }
        public long SyncToken { get; set; }
        public int PageSize { get; set; }
    }
```
