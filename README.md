# agilitycms-dotnet5-fetch-api
Agility package to pull content from Agility instance via Fetch API.

**Note:** Agility CMS .NET Fetch API does not include page management or URL redirections - this functionality exists in Agility CMS .NET Core repo https://github.com/agility/agilitycms-dotnet5-core

# Setup
1. Clone  Agility CMS .NET Fetch API repo at https://github.com/agility/agilitycms-dotnet5-fetch-api
2. Clone and configure Agility CMS .NET 5 Starter at https://github.com/agility/agilitycms-dotnet5-starter
4. Open Agility CMS .NET 5 Starter and add the project  Agility CMS .NET Fetch API to your solution
![image](https://user-images.githubusercontent.com/6853592/125954842-08e47e9b-f244-4d6f-84d4-353bc9345903.png)
4. Finally add a dependency by right clicking 'Dependencies' under Agility.NET.Starter
![image](https://user-images.githubusercontent.com/6853592/125955180-eebb9395-c807-48be-a355-6f32eff63b0c.png)
5. Select 'Add Project Reference' and check Agility.NET.FetchAPI
![image](https://user-images.githubusercontent.com/6853592/125955314-6fbb290c-2752-4481-9b74-4dd976bb3d25.png)

# Fetch API
The Fetch API supports the following calls.
Function | Parameters | Description
:--- | :--- | :--- 
GetContentItem | ```GetItemParameters``` | Get a single content item
GetContentList | ```GetListParameters``` | Get a content list
GetGallery | ```GetGalleryParameters``` | Get a gallery
GetPage |  ```GetPageParameters``` | Get a page
GetSitemapFlat | ```GetSitemapParameters``` | Get a flat sitemap
GetSitemapNested | ```GetSitemapParameters``` | Get a nested sitemap
GetUrlRedirections | ```GetUrlRedirectionsParameters``` | Get URL redirections
GetSyncContent | ```GetSyncParameters``` | Grab all content using a sync token
GetSyncPages | ```GetSyncParameters``` | Get all pages using a sync token

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







