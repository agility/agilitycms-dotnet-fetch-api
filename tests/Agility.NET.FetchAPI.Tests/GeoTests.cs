using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.Extensions.Options;

namespace Agility.NET.FetchAPI.Tests;

/// <summary>
/// A page's GEO / AEO properties and the newer SEO properties (PROD-2726), read from a canned response:
/// no network or credentials needed.
/// </summary>
public class GeoTests
{
    private const string PageWithGeo = """
        {
          "pageID": 7, "name": "about", "path": "/about", "title": "About", "pageType": "static",
          "seo": { "metaDescription": "About us", "metaKeywords": "", "metaHTML": "", "menuVisible": null, "sitemapVisible": null,
                   "title": "About us | Example", "canonicalUrl": "https://example.com/about",
                   "robots": { "index": true, "follow": false }, "ogImage": "https://cdn.example.com/share.png" },
          "geo": { "schemaType": "AboutPage", "summary": "Who we are.",
                   "structuredData": { "@context": "https://schema.org", "@graph": [{ "@type": "Organization", "name": "Example" }] },
                   "ai": { "llmsInclude": "include", "crawl": "block" }, "futureKey": 1 },
          "zones": {}
        }
        """;

    private const string PageWithoutGeo = """
        {
          "pageID": 8, "name": "home", "path": "/home", "title": "Home", "pageType": "static",
          "seo": { "metaDescription": "Home", "metaKeywords": "", "metaHTML": "", "menuVisible": null, "sitemapVisible": null },
          "zones": {}
        }
        """;

    private static FetchApiService Service(string json) =>
        new(new HttpClient(new FakeHandler(json)), Options.Create(new AppSettings
        {
            InstanceGUID = "1234abcd-u",
            FetchAPIKey = "fetch-key",
            PreviewAPIKey = "preview-key",
        }));

    private sealed class FakeHandler(string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
    }

    private static Task<Models.API.PageResponse> GetPage(string json) =>
        Service(json).GetTypedPage(new GetPageParameters { Locale = "en-us", PageId = 7 });

    [Fact]
    public async Task A_page_carries_its_geo_properties()
    {
        var page = await GetPage(PageWithGeo);

        Assert.NotNull(page.Geo);
        Assert.Equal("AboutPage", page.Geo.SchemaType);
        Assert.Equal("Who we are.", page.Geo.Summary);
        Assert.Equal("include", page.Geo.Ai.LlmsInclude);
        Assert.Equal("block", page.Geo.Ai.Crawl);

        var structuredData = Assert.NotNull(page.Geo.StructuredData);
        Assert.Equal(JsonValueKind.Object, structuredData.ValueKind);
        Assert.Equal("Organization", structuredData.GetProperty("@graph")[0].GetProperty("@type").GetString());
    }

    [Fact]
    public async Task A_page_carries_the_new_seo_properties()
    {
        var page = await GetPage(PageWithGeo);

        Assert.Equal("About us", page.SEO.MetaDescription);
        Assert.Equal("About us | Example", page.SEO.Title);
        Assert.Equal("https://example.com/about", page.SEO.CanonicalUrl);
        Assert.Equal("https://cdn.example.com/share.png", page.SEO.OgImage);
        Assert.True(page.SEO.Robots.Index);
        Assert.False(page.SEO.Robots.Follow);
    }

    [Fact]
    public async Task A_page_without_geo_data_has_null_geo_and_seo_properties()
    {
        var page = await GetPage(PageWithoutGeo);

        Assert.Equal(8, page.PageID);
        Assert.Null(page.Geo);
        Assert.Null(page.SEO.Title);
        Assert.Null(page.SEO.CanonicalUrl);
        Assert.Null(page.SEO.Robots);
        Assert.Null(page.SEO.OgImage);
    }

    [Fact]
    public async Task Structured_data_can_be_an_array()
    {
        var page = await GetPage(PageWithGeo.Replace(
            """{ "@context": "https://schema.org", "@graph": [{ "@type": "Organization", "name": "Example" }] }""",
            """[{ "@type": "WebPage" }, { "@type": "BreadcrumbList" }]"""));

        var structuredData = Assert.NotNull(page.Geo.StructuredData);
        Assert.Equal(JsonValueKind.Array, structuredData.ValueKind);
        Assert.Equal(2, structuredData.GetArrayLength());
    }

    private const string ItemWithGeo = """
        {
          "contentID": 42,
          "properties": { "state": 2, "versionID": 3, "referenceName": "posts", "definitionName": "Post", "itemOrder": 0 },
          "fields": { "title": "A post" },
          "seo": { "metaDescription": "The post", "metaKeywords": "", "metaHTML": "", "menuVisible": null, "sitemapVisible": null },
          "geo": { "schemaType": "ItemPage", "summary": "The post in one sentence.",
                   "structuredData": { "@type": "BlogPosting", "headline": "A post" },
                   "ai": { "crawl": "block" } }
        }
        """;

    private sealed class PostFields
    {
        public string? Title { get; set; }
    }

    [Fact]
    public async Task A_dynamic_page_list_item_carries_its_geo_properties()
    {
        var item = await Service(ItemWithGeo).GetTypedContentItem<PostFields>(new GetItemParameters { Locale = "en-us", ContentId = 42 });

        Assert.Equal("A post", item.Fields!.Title);
        Assert.NotNull(item.Geo);
        Assert.Equal("ItemPage", item.Geo!.SchemaType);
        Assert.Equal("The post in one sentence.", item.Geo.Summary);
        Assert.Equal("block", item.Geo.Ai.Crawl);
        Assert.Null(item.Geo.Ai.LlmsInclude);
        Assert.Equal("BlogPosting", item.Geo.StructuredData!.Value.GetProperty("@type").GetString());
    }

    [Fact]
    public async Task An_item_without_geo_data_has_null_geo()
    {
        var item = await Service("""{ "contentID": 42, "fields": { "title": "A post" }, "seo": { "metaDescription": "The post" } }""")
            .GetTypedContentItem<PostFields>(new GetItemParameters { Locale = "en-us", ContentId = 42 });

        Assert.Equal(42, item.ContentID);
        Assert.Null(item.Geo);
    }

    [Fact]
    public async Task Typed_content_list_items_carry_geo()
    {
        var list = await Service($$"""{ "items": [{{ItemWithGeo}}, { "contentID": 43, "fields": { "title": "No GEO" } }], "totalCount": 2 }""")
            .GetTypedContentList<PostFields>(new GetListParameters { Locale = "en-us", ReferenceName = "posts", Take = 10 });

        Assert.Equal("ItemPage", list.Items[0].Geo.SchemaType);
        Assert.Null(list.Items[1].Geo);
    }

    // GetContentByGraphQL deserializes with Newtonsoft.Json, not System.Text.Json.
    private static Task<List<Models.API.ContentItemResponse<PostFields>>> GetByGraphQL(string itemJson) =>
        Service($$"""{ "data": { "posts": [{{itemJson}}] } }""")
            .GetContentByGraphQL<PostFields>("{ posts { contentID } }", "en-us", "posts");

    [Fact]
    public async Task GraphQL_reads_structured_data_as_an_object()
    {
        var items = await GetByGraphQL(ItemWithGeo.Replace(
            "\"headline\": \"A post\"",
            "\"headline\": \"A post\", \"datePublished\": \"2026-10-16T09:30:00+02:00\", \"ratingValue\": 4.10"));

        var item = Assert.Single(items);
        Assert.Equal("ItemPage", item.Geo!.SchemaType);
        Assert.Equal("block", item.Geo.Ai.Crawl);
        var structuredData = Assert.NotNull(item.Geo.StructuredData);
        Assert.Equal(JsonValueKind.Object, structuredData.ValueKind);
        Assert.Equal("BlogPosting", structuredData.GetProperty("@type").GetString());
        // Date-like strings come through as written; numbers keep their value.
        Assert.Equal("2026-10-16T09:30:00+02:00", structuredData.GetProperty("datePublished").GetString());
        Assert.Equal(4.1, structuredData.GetProperty("ratingValue").GetDouble());
    }

    [Fact]
    public async Task GraphQL_reads_very_large_and_very_small_numbers_in_structured_data()
    {
        // Read as decimal, 1e30 is out of range and failed the whole response, and 1e-40 became 0.
        var items = await GetByGraphQL(ItemWithGeo.Replace(
            "\"headline\": \"A post\"",
            "\"headline\": \"A post\", \"big\": 1e30, \"small\": 1e-40, \"price\": 19.99"));

        var structuredData = Assert.NotNull(Assert.Single(items).Geo!.StructuredData);
        Assert.Equal(1e30, structuredData.GetProperty("big").GetDouble());
        Assert.Equal(1e-40, structuredData.GetProperty("small").GetDouble());
        Assert.Equal(19.99, structuredData.GetProperty("price").GetDouble());
    }

    [Fact]
    public async Task GraphQL_reads_structured_data_as_an_array()
    {
        var items = await GetByGraphQL(ItemWithGeo.Replace(
            """{ "@type": "BlogPosting", "headline": "A post" }""",
            """[{ "@type": "WebPage" }, { "@type": "BreadcrumbList" }]"""));

        var structuredData = Assert.NotNull(Assert.Single(items).Geo!.StructuredData);
        Assert.Equal(JsonValueKind.Array, structuredData.ValueKind);
        Assert.Equal(2, structuredData.GetArrayLength());
        Assert.Equal("BreadcrumbList", structuredData[1].GetProperty("@type").GetString());
    }

    [Fact]
    public async Task GraphQL_reads_null_or_absent_structured_data_and_geo()
    {
        var withNull = Assert.Single(await GetByGraphQL(ItemWithGeo.Replace(
            """{ "@type": "BlogPosting", "headline": "A post" }""", "null")));
        Assert.NotNull(withNull.Geo);
        Assert.Null(withNull.Geo!.StructuredData);

        var absent = Assert.Single(await GetByGraphQL("""{ "contentID": 43, "geo": { "summary": "No JSON-LD." } }"""));
        Assert.Equal("No JSON-LD.", absent.Geo!.Summary);
        Assert.Null(absent.Geo.StructuredData);

        var noGeo = Assert.Single(await GetByGraphQL("""{ "contentID": 44, "fields": { "title": "No GEO" } }"""));
        Assert.Null(noGeo.Geo);
    }

    [Fact]
    public void Newtonsoft_writes_structured_data_back_as_json()
    {
        var geo = new Models.GeoProperties
        {
            StructuredData = JsonDocument.Parse("""[{ "@type": "WebPage" }]""").RootElement.Clone(),
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(geo);
        Assert.Contains("\"StructuredData\":[{\"@type\":\"WebPage\"}]", json);

        var roundTripped = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.GeoProperties>(json)!;
        Assert.Equal(JsonValueKind.Array, roundTripped.StructuredData!.Value.ValueKind);

        Assert.Contains("\"StructuredData\":null", Newtonsoft.Json.JsonConvert.SerializeObject(new Models.GeoProperties()));
    }

    [Fact]
    public void Typed_sync_items_keep_their_seo_and_geo()
    {
        var sync = DynamicHelpers.DeserializeTo<Models.SyncContentItem>(
            $$"""{ "syncToken": 5, "items": [{{ItemWithGeo}}, { "contentID": 43, "properties": { "definitionName": "Post" }, "fields": { "title": "No GEO" } }] }""");

        var items = DynamicHelpers.DeserializeSyncItemsTo<PostFields>(sync.Items, "Post");

        Assert.Equal(2, items.Count);
        Assert.Equal("A post", items[0].Fields!.Title);
        Assert.Equal("The post", items[0].SEO!.MetaDescription);
        Assert.Equal("ItemPage", items[0].Geo!.SchemaType);
        Assert.Equal("BlogPosting", items[0].Geo!.StructuredData!.Value.GetProperty("@type").GetString());
        Assert.Null(items[1].SEO);
        Assert.Null(items[1].Geo);
    }
}
