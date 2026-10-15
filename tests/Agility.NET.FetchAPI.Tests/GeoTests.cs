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
}
