using Xunit;
using Agility.NET.FetchAPI.Models.Data;
using Newtonsoft.Json.Linq;

namespace Agility.NET.FetchAPI.Tests;

[Collection("Agility API")]
public class PageTests
{
    private readonly ServiceFixture _fixture;

    public PageTests(ServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetPage_FromSitemap_ReturnsPageWithZones()
    {
        // Arrange - First get the sitemap to find a page
        var sitemapParams = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        var sitemap = await _fixture.FetchService.GetTypedSitemapFlat(sitemapParams);
        Assert.NotNull(sitemap);
        Assert.NotEmpty(sitemap);

        // Get the first page from the sitemap
        var firstPage = sitemap.First();
        Assert.True(firstPage.PageID > 0, "First page should have a valid PageID");

        // Act - Get the page details
        var pageParams = new GetPageParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            PageId = firstPage.PageID,
            ContentLinkDepth = 2,
            ExpandAllContentLinks = true
        };

        var pageResult = await _fixture.FetchService.GetPage(pageParams);

        // Assert
        Assert.NotNull(pageResult);
        Assert.NotEmpty(pageResult);

        var pageJson = JObject.Parse(pageResult);
        Assert.NotNull(pageJson);
    }

    [Fact]
    public async Task GetTypedPage_FromSitemap_ReturnsTypedPage()
    {
        // Arrange - First get the sitemap to find a page
        var sitemapParams = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        var sitemap = await _fixture.FetchService.GetTypedSitemapFlat(sitemapParams);
        Assert.NotNull(sitemap);
        Assert.NotEmpty(sitemap);

        var firstPage = sitemap.First();

        // Act - Get the typed page (ContentLinkDepth must be 0 for typed pages)
        var pageParams = new GetPageParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            PageId = firstPage.PageID,
            ContentLinkDepth = 0
        };

        var pageResult = await _fixture.FetchService.GetTypedPage(pageParams);

        // Assert
        Assert.NotNull(pageResult);
        Assert.True(pageResult.PageID > 0, "PageID should be valid");
    }

    [Fact]
    public async Task GetPage_WithContentZones_ReturnsModules()
    {
        // Arrange - Get sitemap first
        var sitemapParams = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        var sitemap = await _fixture.FetchService.GetTypedSitemapFlat(sitemapParams);
        Assert.NotNull(sitemap);
        Assert.NotEmpty(sitemap);

        var firstPage = sitemap.First();

        // Act - Get the page with zones
        var pageParams = new GetPageParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            PageId = firstPage.PageID,
            ContentLinkDepth = 3,
            ExpandAllContentLinks = true
        };

        var pageResult = await _fixture.FetchService.GetPage(pageParams);

        // Assert
        Assert.NotNull(pageResult);

        var pageJson = JObject.Parse(pageResult);
        var zones = pageJson["zones"];

        // Log info about zones for debugging
        if (zones != null)
        {
            foreach (var zone in zones.Children<JProperty>())
            {
                var zoneArray = zone.Value as JArray;
                if (zoneArray != null && zoneArray.Count > 0)
                {
                    // We found a zone with modules
                    var firstModule = zoneArray[0];
                    Assert.NotNull(firstModule);

                    // Check module has expected properties
                    Assert.NotNull(firstModule["module"]);
                }
            }
        }
    }

    [Fact]
    public async Task GetPage_PreviewMode_ReturnsPage()
    {
        // Arrange - Get sitemap in preview mode
        var sitemapParams = new GetSitemapParameters
        {
            IsPreview = true,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        var sitemap = await _fixture.PreviewService.GetTypedSitemapFlat(sitemapParams);
        Assert.NotNull(sitemap);
        Assert.NotEmpty(sitemap);

        var firstPage = sitemap.First();

        // Act - Get the page in preview mode
        var pageParams = new GetPageParameters
        {
            IsPreview = true,
            Locale = TestConfiguration.Locale,
            PageId = firstPage.PageID,
            ContentLinkDepth = 2
        };

        var pageResult = await _fixture.PreviewService.GetPage(pageParams);

        // Assert
        Assert.NotNull(pageResult);
        Assert.NotEmpty(pageResult);
    }
}
