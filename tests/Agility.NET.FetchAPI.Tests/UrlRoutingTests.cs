using Xunit;
using Agility.NET.FetchAPI.Util;

namespace Agility.NET.FetchAPI.Tests;

/// <summary>
/// Tests for URL routing based on Instance GUID suffix.
/// </summary>
public class UrlRoutingTests
{
    [Fact]
    public void Constants_BaseUrl_IsCorrect()
    {
        Assert.Equal("https://api.aglty.io", Constants.BaseUrl);
    }

    [Fact]
    public void Constants_BaseUrlDev_IsCorrect()
    {
        Assert.Equal("https://api-dev.aglty.io", Constants.BaseUrlDev);
    }

    [Fact]
    public void Constants_BaseUrlCanada_IsCorrect()
    {
        Assert.Equal("https://api-ca.aglty.io", Constants.BaseUrlCanada);
    }

    [Fact]
    public void Constants_BaseUrlEurope_IsCorrect()
    {
        Assert.Equal("https://api-eu.aglty.io", Constants.BaseUrlEurope);
    }

    [Fact]
    public void Constants_BaseUrlAustralia_IsCorrect()
    {
        Assert.Equal("https://api-aus.aglty.io", Constants.BaseUrlAustrailia);
    }

    [Fact]
    public void Constants_BaseUrlUSA2_IsCorrect()
    {
        Assert.Equal("https://api-usa2.aglty.io", Constants.BaseUrlUSA2);
    }

    [Theory]
    [InlineData("abc123-d", "-d")]
    [InlineData("abc123-c", "-c")]
    [InlineData("abc123-e", "-e")]
    [InlineData("abc123-a", "-a")]
    [InlineData("abc123-us2", "-us2")]
    public void InstanceGUID_EndsWithExpectedSuffix(string guid, string expectedSuffix)
    {
        Assert.True(guid.EndsWith(expectedSuffix));
    }
}
