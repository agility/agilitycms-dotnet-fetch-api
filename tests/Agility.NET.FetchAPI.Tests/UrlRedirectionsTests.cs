using Xunit;
using Agility.NET.FetchAPI.Models.Data;

namespace Agility.NET.FetchAPI.Tests;

[Collection("Agility API")]
public class UrlRedirectionsTests
{
    private readonly ServiceFixture _fixture;

    public UrlRedirectionsTests(ServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetUrlRedirections_ReturnsResponse()
    {
        // Arrange
        var parameters = new GetUrlRedirectionsParameters
        {
            LastAccessDate = null
        };

        // Act
        var result = await _fixture.FetchService.GetUrlRedirections(parameters);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUrlRedirections_WithLastAccessDate_ReturnsResponse()
    {
        // Arrange
        var parameters = new GetUrlRedirectionsParameters
        {
            LastAccessDate = DateTime.UtcNow.AddDays(-30)
        };

        // Act
        var result = await _fixture.FetchService.GetUrlRedirections(parameters);

        // Assert
        Assert.NotNull(result);
    }
}
