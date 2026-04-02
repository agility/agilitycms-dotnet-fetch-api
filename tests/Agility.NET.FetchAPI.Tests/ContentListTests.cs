using Xunit;
using Agility.NET.FetchAPI.Models.Data;
using Newtonsoft.Json.Linq;

namespace Agility.NET.FetchAPI.Tests;

[Collection("Agility API")]
public class ContentListTests
{
    private readonly ServiceFixture _fixture;

    public ContentListTests(ServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetContentList_FetchMode_ReturnsContent()
    {
        // Arrange
        var parameters = new GetListParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ReferenceName = TestConfiguration.TestListReferenceName,
            Take = 10
        };

        // Act
        var result = await _fixture.FetchService.GetContentList(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var json = JObject.Parse(result);
        Assert.NotNull(json["items"]);
    }

    [Fact]
    public async Task GetContentList_PreviewMode_ReturnsContent()
    {
        // Arrange
        var parameters = new GetListParameters
        {
            IsPreview = true,
            Locale = TestConfiguration.Locale,
            ReferenceName = TestConfiguration.TestListReferenceName,
            Take = 10
        };

        // Act
        var result = await _fixture.PreviewService.GetContentList(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var json = JObject.Parse(result);
        Assert.NotNull(json["items"]);
    }

    [Fact]
    public async Task GetContentList_WithTakeAndSkip_ReturnsPaginatedContent()
    {
        // Arrange
        var parameters = new GetListParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ReferenceName = TestConfiguration.TestListReferenceName,
            Take = 5,
            Skip = 0
        };

        // Act
        var result = await _fixture.FetchService.GetContentList(parameters);

        // Assert
        Assert.NotNull(result);

        var json = JObject.Parse(result);
        var items = json["items"] as JArray;
        Assert.NotNull(items);
        Assert.True(items.Count <= 5, "Should return at most 5 items");
    }

    [Fact]
    public async Task GetTypedContentList_ReturnsTypedContent()
    {
        // Arrange
        var parameters = new GetListParameters
        {
            IsPreview = false,
            Locale = TestConfiguration.Locale,
            ReferenceName = TestConfiguration.TestListReferenceName,
            Take = 10
        };

        // Act
        var result = await _fixture.FetchService.GetTypedContentList<dynamic>(parameters);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
    }
}
