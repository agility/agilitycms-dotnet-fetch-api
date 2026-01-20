using Xunit;
using Agility.NET.FetchAPI.Models.Data;
using Newtonsoft.Json.Linq;

namespace Agility.NET.FetchAPI.Tests;

[Collection("Agility API")]
public class SitemapTests
{
    private readonly ServiceFixture _fixture;

    public SitemapTests(ServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetSitemapFlat_FetchMode_ReturnsSitemap()
    {
        // Arrange
        var parameters = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        // Act
        var result = await _fixture.FetchService.GetSitemapFlat(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetSitemapFlat_PreviewMode_ReturnsSitemap()
    {
        // Arrange
        var parameters = new GetSitemapParameters
        {
            IsPreview = true,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        // Act
        var result = await _fixture.PreviewService.GetSitemapFlat(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetSitemapNested_FetchMode_ReturnsSitemap()
    {
        // Arrange
        var parameters = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        // Act
        var result = await _fixture.FetchService.GetSitemapNested(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetTypedSitemapFlat_ReturnsTypedSitemap()
    {
        // Arrange
        var parameters = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        // Act
        var result = await _fixture.FetchService.GetTypedSitemapFlat(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetTypedSitemapNested_ReturnsTypedSitemap()
    {
        // Arrange
        var parameters = new GetSitemapParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        // Act
        var result = await _fixture.FetchService.GetTypedSitemapNested(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}
